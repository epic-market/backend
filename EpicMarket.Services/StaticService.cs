using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class StaticService : IStaticService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public StaticService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }
        public Task<List<DropDownOptions>> BusinessCategoriesOptions()
        {
            return _context.BusinessCategories.Select(c => new DropDownOptions { Text = c.Name, Value = c.ID }).ToListAsync();
        }
        public Task<List<DropDownOptions>> GetStatusOptions()
        {
            return _context.StatusOptionSets.Select(c => new DropDownOptions { Text = c.Status, Value = c.Id }).ToListAsync();
        }
        public Task<List<DropDownOptions>> GetOderStatusOptions()
        {
            return _context.OrderStatusOptions.Select(c => new DropDownOptions { Text = c.OrderStatus, Value = c.Id }).ToListAsync();
        }
        public Task<List<DropDownOptions>> GetOderTypeOptions()
        {
            return _context.OrderTypesOptions.Select(c => new DropDownOptions { Text = c.Ordertype, Value = c.Id }).ToListAsync();
        }
        public Task<List<DropDownOptions>> GetAllblogCategories()
        {
            return _context.BlogCategory.Select(c => new DropDownOptions { Text = c.Name, Value = c.Id }).ToListAsync();
        }
        public Task<List<DropDownOptions>> GetAllSupportCategorys()
        {
            return _context.TaskTypes.Select(c => new DropDownOptions { Text = c.Name, Value = c.ID }).ToListAsync();
        }
    }
}
