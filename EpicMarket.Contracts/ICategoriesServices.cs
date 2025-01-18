using EpicMarket.Data.Models;
using EpicMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Contracts
{
    public interface ICategoryService
    {
        Task<List<CategoriesDto>> GetCategories();

        Task<CategoriesDto> GetCategory(int id);
        Task<CategoriesDto> CreateCategory(CategoriesDto category);
        Task<CategoriesDto> UpdateCategory(int id, UpdateCategoryDto category);
        Task<CategoriesDto> DeleteCategory(int id);
    }
}
