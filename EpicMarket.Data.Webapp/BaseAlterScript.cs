using EpicMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Webapp
{
    public class BaseAlterScript 
    {
        private readonly ApplicationDbContext dbContext;

        public BaseAlterScript(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void updateDatabaseVersion(string versionClass)
        {
            var databaseVersion = dbContext.DatabaseVersions.FirstOrDefault(d => d.VersionClass == versionClass);
            databaseVersion.Status = true;
            dbContext.DatabaseVersions.Update(databaseVersion);
            dbContext.SaveChanges();
        }
    }
}
