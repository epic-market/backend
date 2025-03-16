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
    public class AlterScript12 : BaseAlterScript,IAlterScript
    {
        private readonly ApplicationDbContext dbContext;

        public AlterScript12(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }



        public void Execute()
        {

            Console.WriteLine("Started Executing " + this.GetType().Name);

            if (!dbContext.AttachmentTypes.Any(cm => cm.Name == "BusinessCategory"))
            {
                var entity = new AttachmentType
                {
                    Name = "BusinessCategory",
                    Description = "BusinessCategory",
                };
                dbContext.AttachmentTypes.Add(entity);
            }

           
             dbContext.SaveChanges();

            this.updateDatabaseVersion(this.GetType().Name);

            Console.WriteLine("Completed Executing " + this.GetType().Name);
        }

        //update the same row that it is complete the running 

    }
}
