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
    public class CatalogCategoryService :  ICatalogCategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly IAddressService addressService;
        private readonly IEventLogService eventLogService;
        private readonly ICommunicationQueueService communicationQueueService;
        private readonly ITasksService tasksService;
        private readonly IUnitOfWork unitOfWork;
		public CatalogCategoryService(
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
            var categories = await _context.CatalogCategories.Where(c => c.BusinessID == businessId).ToListAsync();
            return mapper.Map<List<CategoriesDto>>(categories);
        }     

        public async Task<CategoriesDto> GetCategory(int id)
        {
            var category = await _context.CatalogCategories.FindAsync(id);
            return mapper.Map<CategoriesDto>(category);
        }

        public async Task<CategoriesDto> CreateCategory(CategoriesDto category)
        {
            try
            {
                var newCategory = mapper.Map<CatalogCategory>(category);
                _context.CatalogCategories.Add(newCategory);
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
            var updatedCategory = await _context.CatalogCategories.FindAsync(id);
            if (updatedCategory == null)
            {
                throw new Exception("Category not found");
            }
            updatedCategory.Name = category.Name;
            updatedCategory.Description = category.Description;
            _context.CatalogCategories.Update(updatedCategory);
            await _context.SaveChangesAsync();
            return mapper.Map<CategoriesDto>(updatedCategory);
        }

        public async Task<CategoriesDto> DeleteCategory(int id)
        {
            var category = await _context.CatalogCategories.FindAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }
            _context.CatalogCategories.Remove(category);
            await _context.SaveChangesAsync();
            return mapper.Map<CategoriesDto>(category);
        }

    }
}
