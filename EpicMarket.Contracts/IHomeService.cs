using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface IHomeService
    {
        Task<List<FaqCategoryDto>> GetAllFaqCategoryAsync(string typeOfFAQ);

        Task<List<FaqDto>> GetAllFaqByCategoryAsync(int Category);

        Task<List<BlogDto>> GetAllBlogs(BlogParams blogParams);
        Task<List<BlogDto>> GetAllBlogsByCategory(BlogsByCategoryParams blogParams);

        Task<BlogDto> GetBlogDetails(int blogId);

        Task<List<FaqDto>> GetAllFaqsCustomerAsync();

        Task<List<CategoryDto>> GetAllCategories(CategoryParams categoryParams);

        Task<List<TrendingBusinessDto>> GetTrendingBusinesses(TrendingBusinessParams trendingBusinessParams);
        
    }
}
