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
            var hasPurchased = await _context.OrderDetails.Include(c=>c.Order.Person).AnyAsync(oi => oi.Order.Person.UserName == CustomerUserName && oi.CatalogID == request.ProductId);

            if (!hasPurchased) {
                throw new UnauthorizedAccessException("Can only rate purchased products");
            }
                
            var CustomerID = _context.Users.FirstOrDefault(c => c.UserName == CustomerUserName).Id;

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


            var CatelogProduct = _context.Catalogs.Find(request.ProductId);
            if (CatelogProduct != null) {
                var NewAverage = ((CatelogProduct.Rating * CatelogProduct.ReviewCount) + request.Rating) / CatelogProduct.ReviewCount + 1;
                CatelogProduct.Rating = NewAverage;
                CatelogProduct.ReviewCount = CatelogProduct.ReviewCount + 1;
                _context.Update(CatelogProduct);
                await _context.SaveChangesAsync(); 
            
            }
        }

        public async Task AddOutletRatingAsync(AddOutletRatingRequest request, string CustomerUserName)
        {
            // Validate if customer has ordered from this outlet
            var hasOrdered = await _context.Orders.Include(c => c.Person).AnyAsync(oi => oi.Person.UserName == CustomerUserName && oi.OutletID == request.OutletID);

            if (!hasOrdered)
                throw new UnauthorizedAccessException("Can only rate outlets you've ordered from");



            var CustomerID = _context.Users.FirstOrDefault(c => c.UserName == CustomerUserName).Id;

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

            var Outlet = _context.Outlets.Find(request.OutletID);
            if (Outlet != null)
            {
                var NewAverage = ((Outlet.Rating * Outlet.ReviewCount) + request.Rating) / Outlet.ReviewCount + 1;
                Outlet.Rating = NewAverage;
                Outlet.ReviewCount = Outlet.ReviewCount + 1;
                _context.Update(Outlet);
                await _context.SaveChangesAsync();
            }
        }

    }

}
