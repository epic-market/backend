//boilder plate for entity service  

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
    public class EntityService : IEntityService
    {
        private readonly ApplicationDbContext _context;
		private readonly IUnitOfWork unitOfWork;

		public EntityService(ApplicationDbContext context,IUnitOfWork unitOfWork)
        {
            _context = context;
			this.unitOfWork = unitOfWork;
		}


		public async Task<int> GetEntityId(string entityName)
		{
			var entity = await _context.Entity.FirstOrDefaultAsync(e => e.Name == entityName);
			if (entity == null)
			{
				throw new Exception("Entity not found");
			}
			return entity.ID;
		}
    }

 
}
