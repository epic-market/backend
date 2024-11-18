using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using Microsoft.EntityFrameworkCore;
using NVelocity.Runtime.Directive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class SearchService : ISearchService
    {
        private readonly ApplicationDbContext _context;
        private const double EarthRadiusKm = 6371;
        private const int MaxProducts = 6;
        private const int MaxBranches = 4;

        public SearchService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<SearchResult> SearchAsync(SearchRequest request)
        {

            var searchTerm = request.SearchTerm?.Trim().ToLower();

            if (string.IsNullOrEmpty(searchTerm))
                return new SearchResult();

            var branchQuery = _context.Outlets.Include(c=>c.Address).AsQueryable();
            var productQuery = _context.OutletProducts.Include(p => p.Outlet).Include(p=>p.Product).Include(p => p.Outlet.Address).AsQueryable();

            // Apply location filter if coordinates are provided
            if (request.Latitude.HasValue && request.Longitude.HasValue)
            {
                branchQuery = branchQuery.Where(b =>
                    CalculateDistance(
                        request.Latitude.Value,
                        request.Longitude.Value,
                        b.Address.Latitude,
                        b.Address.Latitude) <= request.RadiusKm);

                productQuery = productQuery.Where(p =>
                    CalculateDistance(
                        request.Latitude.Value,
                        request.Longitude.Value,
                        p.Outlet.Address.Latitude,
                        p.Outlet.Address.Longitude) <= request.RadiusKm);
            }

            // Apply text search
            var branches = await branchQuery
                .Where(b => b.Name.ToLower().Contains(searchTerm))
                .Select(b => new BranchSearchDto
                {
                    Id = b.ID,
                    Name = b.Name,
                    Description = b.Description,
                    Latitude = b.Address.Latitude,
                    Longitude = b.Address.Longitude,
                    Distance = request.Latitude.HasValue ?
                        CalculateDistance(
                            request.Latitude.Value,
                            request.Longitude.Value,
                            b.Address.Latitude,
                            b.Address.Longitude) : 0
                })
                 .OrderBy(b => b.Distance)
                .Take(MaxBranches)
                .ToListAsync();

            var products = await productQuery
                .Where(p => p.Product.Name.ToLower().Contains(searchTerm))
                .Select(p => new ProductSearchDto 
                {
                    Id = p.Product.ID,
                    Name = p.Product.Name,
                    Price = p.Product.Rate,
                    BranchName = p.Outlet.Name,
                    BranchId = p.Outlet.ID,
                    Distance = request.Latitude.HasValue ?
                        CalculateDistance(
                            request.Latitude.Value,
                            request.Longitude.Value,
                            p.Outlet.Address.Latitude,
                            p.Outlet.Address.Longitude) : 0
                })
               .OrderBy(p => p.Distance)
                .Take(MaxProducts)
                .ToListAsync();

            return new SearchResult
            {
                Branches = branches.OrderBy(b => b.Distance).ToList(),
                Products = products.OrderBy(p => p.Distance).ToList()
            };
        }


        public async Task<List<TrendingOutletDto>> GetTop3TrendingStoresNearbyAsync(
        double latitude,
        double longitude,
        double radiusKm = 10)
        {
            var last24Hours = DateTime.UtcNow.AddHours(-24);

            var trendingStores = await _context.Orders
                .Include(o => o.OrderStatusOptions)
                .Include(o => o.Outlet)
                .Include(o => o.Outlet.Address)
                .Where(o => o.OrderAt >= last24Hours &&
                           o.OrderStatusOptions.OrderStatus != OrderStatusConstants.Canceled)
                .Where(o => CalculateDistance(
                    latitude,
                    longitude,
                    o.Outlet.Address.Latitude,
                    o.Outlet.Address.Longitude) <= radiusKm)
                .GroupBy(o => new
                {
                    o.OutletID,
                    o.Outlet.Name,
                    o.Outlet.Address.Latitude,
                    o.Outlet.Address.Longitude
                })
                .Select(g => new TrendingOutletDto
                {
                    OutletId = g.Key.OutletID,
                    OutletName = g.Key.Name,
                    OrderCount = g.Count(),
                    LastOrderTime = g.Max(o => o.OrderAt),
                    Distance = CalculateDistance(
                        latitude,
                        longitude,
                        g.Key.Latitude,
                        g.Key.Longitude)
                })
                .OrderByDescending(s => s.OrderCount)
                .Take(3)
                .ToListAsync();

            return trendingStores;
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var dLat = ToRadian(lat2 - lat1);
            var dLon = ToRadian(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Asin(Math.Sqrt(a));
            return EarthRadiusKm * c;
        }

        private double ToRadian(double degree)
        {
            return degree * Math.PI / 180;
        }
    }
}
