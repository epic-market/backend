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
    public class AlterScript7 : BaseAlterScript,IAlterScript
    {
        private readonly ApplicationDbContext dbContext;

        public AlterScript7(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }



        public void Execute()
        {

            Console.WriteLine("Started Executing " + this.GetType().Name);

           
            if (!dbContext.ApplicationConfigurations.Any(cm => cm.Name == "PRODUCTINTERNAL"))
            {
                var applicationConfiguration = new ApplicationConfiguration
                {
                    Name = "PRODUCTINTERNAL",
                    Value = "PRODUCTINTERNAL",
                };

                dbContext.ApplicationConfigurations.Add(applicationConfiguration);
                dbContext.SaveChanges();
            }

            if (!dbContext.AttachmentTypes.Any(cm => cm.Name == "ProductInternal"))
            {
                var personType = new AttachmentType
                {
                    Name = "ProductInternal",
                    Description = "ProductInternal",
                };

                dbContext.AttachmentTypes.Add(personType);
                dbContext.SaveChanges();
            }


            if (!dbContext.Entity.Any(cm => cm.Name == "ProductInternal"))
            {
                var entity = new Entity
                {
                    Name = "ProductInternal",
                    Description = "ProductInternal",
                };

                dbContext.Entity.Add(entity);
                dbContext.SaveChanges();
            }

            this.updateDatabaseVersion(this.GetType().Name);

            Console.WriteLine("Completed Executing " + this.GetType().Name);
        }

        //update the same row that it is complete the running 

    }
}
