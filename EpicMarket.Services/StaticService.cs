using AutoMapper;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Entities;
using EpicMarket.Entities.CustomModels;
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
		private readonly IUnitOfWork unitOfWork;

		public StaticService(ApplicationDbContext context, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _context = context;
            this.mapper = mapper;
			this.unitOfWork = unitOfWork;
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
        public Task<List<DropDownOptions>> GetAllPersonTypes()
        {
            return _context.PersonTypes.Select(c => new DropDownOptions { Text = c.Description, Value = c.ID }).ToListAsync();
        }
        public Task<List<DropDownOptions>> GetAllSupportQuery(int personTypeId)
        {
            return _context.SupportQuerys.Where(c => c.TypeofPersonid == personTypeId).Select(c => new DropDownOptions { Text = c.Query, Value = c.ID }).ToListAsync();
        }

        public async Task<List<DropDownOptions>> GetOrderStatusOptions()
        {
            return await _context.OrderStatusOptions.Select(c => new DropDownOptions()
            {
                Text = c.OrderStatus,
                Value = c.Id
            }).ToListAsync();
        }

        public Task<List<HelpItemDTO>> GetHelpItemsforBypage(string pagename)
        {
            return _context.HelpItems
                   .Where(h => h.Pages != null && h.Pages.Name == pagename)
                   .Select(c => new HelpItemDTO
                   {
                       ID = c.ID,
                       Name = c.Name,
                       Title = c.Title,
                       PageID= c.PageID,
                       IsShownOnPage = c.IsShownOnPage,
                       Description = c.Description,

                   })
                   .ToListAsync();
        }
        public async Task<int> SubscribeforOffers( string gmail)
        {
            var newLead = new PromotionalLeads
            {
                Gmail = gmail,
                CreateDate = DateTime.Now,
                Time = DateTime.Now.TimeOfDay,
                WhichApplication = "userface application" // For now hardcoded
            };

            _context.PromotionalLeads.Add(newLead);
			await unitOfWork.Complete();
			return newLead.Id;

        }
    }
}
