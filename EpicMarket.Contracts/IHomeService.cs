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
        Task<List<FaqCategoryDto>> GetAllFaqCategoryAsync();

        Task<List<FaqDto>> GetAllFaqByCategoryAsync(int CategoryId, string search = null);

        Task<List<BlogDto>> GetAllBlogs(BlogParams blogParams);
        Task<List<BlogCategoryDto>> GetAllBlogCategories();

        Task<BlogDto> GetBlogDetails(int blogId);

    }
}
