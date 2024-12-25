using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class RatingService : IRatingService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RatingService> _logger;

        public RatingService(ApplicationDbContext context, ILogger<RatingService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddProductRatingAsync(AddProductRatingRequest request,string CustomerUserName)
        {
            // Validate if customer has purchased the product
            // var hasPurchased = await _context.OrderDetails.Include(c=>c.Order.Person).AnyAsync(oi => oi.Order.Person.UserName == CustomerUserName && oi.CatalogID == request.ProductId);

            // if (!hasPurchased) {
            //     throw new UnauthorizedAccessException("Can only rate purchased products");
            // }
                
            var CustomerID = _context.Users.FirstOrDefault(c => c.UserName == CustomerUserName).Id;

            var hasRated = await _context.Ratings.AnyAsync(r => r.CustomerId == CustomerID && r.ProductId == request.ProductId);
            if (hasRated)
                throw new UnauthorizedAccessException("Can only rate products once");

            //we should not allow to rate if the outlet is not active
            var catalog = await _context.Catalogs.IgnoreQueryFilters().Where(o => o.ID == request.ProductId && o.IsActive).FirstOrDefaultAsync();
            if(catalog == null){
                throw new UnauthorizedAccessException("products not found");
            }

            var rating = new Rating
            {
                CustomerId = CustomerID,
                ProductId = request.ProductId,
                Stars = request.Rating,
                Review = request.Review,
                CreatedDate = DateTime.UtcNow,
                IsVerified = true
            };

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            if (catalog != null) {

                 if (catalog != null)
                {
                if (catalog.ReviewCount == null || catalog.ReviewCount == 0)
                {
                    catalog.Rating = request.Rating;
                    catalog.ReviewCount = 1;
                }
                else
                {
                    var NewAverage = ((catalog.Rating * catalog.ReviewCount) + request.Rating) / (catalog.ReviewCount + 1);
                    catalog.Rating = NewAverage;
                    catalog.ReviewCount = catalog.ReviewCount + 1;
                }
                _context.Update(catalog);
                await _context.SaveChangesAsync();
            }
            }
        }

        public async Task AddOutletRatingAsync(AddOutletRatingRequest request, string CustomerUserName)
        {
            // Commented out as per instructions
            // Validate if customer has ordered from this outlet
            // var hasOrdered = await _context.Orders.Include(c => c.Person).AnyAsync(oi => oi.Person.UserName == CustomerUserName && oi.OutletID == request.OutletID);

            // if (!hasOrdered)
            //     throw new UnauthorizedAccessException("Can only rate outlets you've ordered from");

            var CustomerID = _context.Users.FirstOrDefault(c => c.UserName == CustomerUserName).Id;
            //we need to check if the customer have already rated this outlet
            var hasRated = await _context.Ratings.AnyAsync(r => r.CustomerId == CustomerID && r.OutletId == request.OutletID);
            if (hasRated)
                throw new UnauthorizedAccessException("Can only rate outlets once");

            //we should not allow to rate if the outlet is not active
            var outlet = await _context.Outlets.IgnoreQueryFilters().Where(o => o.ID == request.OutletID && o.IsActive).FirstOrDefaultAsync();
            if(outlet == null){
                throw new UnauthorizedAccessException("Outlet not found");
            }

            var rating = new Rating
            {
                CustomerId = CustomerID,
                OutletId = request.OutletID,
                Stars = request.Rating,
                Review = request.Review,
                CreatedDate = DateTime.UtcNow,
                IsVerified = true
            };

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
            if (outlet != null)
            {
                if (outlet.ReviewCount == null || outlet.ReviewCount == 0)
                {
                    outlet.Rating = request.Rating;
                    outlet.ReviewCount = 1;
                }
                else
                {
                    var NewAverage = ((outlet.Rating * outlet.ReviewCount) + request.Rating) / (outlet.ReviewCount + 1);
                    outlet.Rating = NewAverage;
                    outlet.ReviewCount = outlet.ReviewCount + 1;
                }
                _context.Update(outlet);
                await _context.SaveChangesAsync();
            }
        }

    }

}
