using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _context;
		private readonly IProfileService profileService;
		private readonly string loggedInUserName;

		public UnitOfWork(ApplicationDbContext context,IProfileService profileService)
        {
            _context = context;
			this.profileService = profileService;
		}

        public IUserRepository UserRepository => new UserRepository(_context, profileService);

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            _context.ChangeTracker.DetectChanges();
            var changes = _context.ChangeTracker.HasChanges();

            return changes;
        }
    }
}
