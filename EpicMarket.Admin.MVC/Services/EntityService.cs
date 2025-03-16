using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EpicMarket.Admin.MVC.Services
{
    public class EntityService : IEntityService
    {
        private readonly ApplicationDbContext dbContext;
		public EntityService(ApplicationDbContext dbContext)
        {
			this.dbContext = dbContext;
		}

        public async Task<int> GetEntityId( string entityName)
        {
            var entity = await dbContext.Entity.FirstOrDefaultAsync(e => e.Name == entityName);
            return entity.ID;
        }


    }
}