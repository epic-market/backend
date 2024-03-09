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
    }
}
