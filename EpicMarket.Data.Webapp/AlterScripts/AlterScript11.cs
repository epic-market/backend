using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Webapp.AlterScripts
{
    public class AlterScript11 : BaseAlterScript,IAlterScript
    {
        private readonly ApplicationDbContext dbContext;

        public AlterScript11(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }



        public void Execute()
        {

            Console.WriteLine("Started Executing " + this.GetType().Name);

            if (!dbContext.Roles.Any(cm => cm.Name == "root"))
            {
                var entity = new AppRole
                {
                    Name = "root",
                    NormalizedName = "ROOT",
                };
                dbContext.Roles.Add(entity);
            }

            if (!dbContext.Roles.Any(cm => cm.Name == "admin"))
            {
                var entity = new AppRole
                {
                    Name = "admin",
                    NormalizedName = "ADMIN",
                };
                dbContext.Roles.Add(entity);
            }

            if (!dbContext.Roles.Any(cm => cm.Name == "moderator"))
            {
                var entity = new AppRole
                {
                    Name = "moderator",
                    NormalizedName = "MODERATOR",
                };
                dbContext.Roles.Add(entity);
            }

            if (!dbContext.Roles.Any(cm => cm.Name == "support"))
            {
                var entity = new AppRole
                {
                    Name = "support",
                    NormalizedName = "SUPPORT",
                };
                dbContext.Roles.Add(entity);
            }

            if (!dbContext.Roles.Any(cm => cm.Name == "businessOwner"))
            {
                var entity = new AppRole
                {
                    Name = "businessOwner",
                    NormalizedName = "BUSINESSOWNER",
                };
                dbContext.Roles.Add(entity);
            }

            if (!dbContext.Roles.Any(cm => cm.Name == "businessEmployee"))
            {
                var entity = new AppRole
                {
                    Name = "businessEmployee",
                    NormalizedName = "BUSINESSEMPLOYEE",
                };
                dbContext.Roles.Add(entity);
            }

            if (!dbContext.Roles.Any(cm => cm.Name == "member"))
            {
                var entity = new AppRole
                {
                    Name = "member",
                    NormalizedName = "MEMBER",
                };
                dbContext.Roles.Add(entity);
            }
             dbContext.SaveChanges();

            this.updateDatabaseVersion(this.GetType().Name);

            Console.WriteLine("Completed Executing " + this.GetType().Name);
        }

        //update the same row that it is complete the running 

    }
}
