using EpicMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IRatingService
    {
        Task AddProductRatingAsync(AddProductRatingRequest request, string CustomerUserName);
        Task AddOutletRatingAsync(AddOutletRatingRequest request, string CustomerUserName);

        Task<List<ReviewResult>> GetAllReviewsOutlet(int outletId);
        Task<List<ReviewResult>> GetAllReviewsProduct(int productId);
    }
}
