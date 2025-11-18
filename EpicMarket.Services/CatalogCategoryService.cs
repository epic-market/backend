using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace EpicMarket.Services
{
    public class ProductCategoryService :  IProductCategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly IAddressService addressService;
        private readonly IEventLogService eventLogService;
        private readonly ICommunicationQueueService communicationQueueService;
        private readonly ITasksService tasksService;
        private readonly IUnitOfWork unitOfWork;
		public ProductCategoryService(
                                ApplicationDbContext context,
                                IMapper mapper ,
                                IAddressService addressService,
                                IEventLogService eventLogService,
                                ICommunicationQueueService communicationQueueService,
                                ITasksService tasksService,
                                IUnitOfWork unitOfWork)
        {
            _context = context;
            this.mapper = mapper;
            this.addressService = addressService;
            this.eventLogService = eventLogService;
            this.communicationQueueService = communicationQueueService;
            this.tasksService = tasksService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<CategoriesDto>> GetCategories(int businessId)
        {
            var categories = await _context.ProductCategories.Where(c => c.BusinessID == businessId).ToListAsync();
            return mapper.Map<List<CategoriesDto>>(categories);
        }     

        public async Task<List<CategoriesDto>> GetCategoriesByOutletId(int outletId)
        {
            // Get business ID from outlet
            var outlet = await _context.Outlets.FirstOrDefaultAsync(o => o.ID == outletId);
            if (outlet == null)
            {
                throw new Exception("Outlet not found");
            }

            // Get categories for the business
            var categories = await _context.ProductCategories
                .Where(c => c.BusinessID == outlet.BussinessID && c.IsActive)
                .ToListAsync();
                
            return mapper.Map<List<CategoriesDto>>(categories);
        }

        public async Task<CategoriesDto> GetCategory(int id)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            return mapper.Map<CategoriesDto>(category);
        }

        public async Task<CategoriesDto> CreateCategory(CategoriesDto category)
        {
            try
            {
                var newCategory = mapper.Map<CatalogCategory>(category);
                _context.ProductCategories.Add(newCategory);
                await _context.SaveChangesAsync();
                return mapper.Map<CategoriesDto>(newCategory);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating category", ex);
            }
        }   

        public async Task<CategoriesDto> UpdateCategory(int id, UpdateCategoryDto category)
        {
            var updatedCategory = await _context.ProductCategories.FindAsync(id);
            if (updatedCategory == null)
            {
                throw new Exception("Category not found");
            }
            updatedCategory.Name = category.Name;
            updatedCategory.Description = category.Description;
            _context.ProductCategories.Update(updatedCategory);
            await _context.SaveChangesAsync();
            return mapper.Map<CategoriesDto>(updatedCategory);
        }

        public async Task<CategoriesDto> DeleteCategory(int id)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }
            _context.ProductCategories.Remove(category);
            await _context.SaveChangesAsync();
            return mapper.Map<CategoriesDto>(category);
        }

    }
}
