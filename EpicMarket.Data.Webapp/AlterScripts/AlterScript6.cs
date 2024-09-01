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
    public class AlterScript6 : BaseAlterScript,IAlterScript
    {
        private readonly ApplicationDbContext dbContext;

        public AlterScript6(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }



        public void Execute()
        {

            Console.WriteLine("Started Executing " + this.GetType().Name);

            if (!dbContext.Event.Any(cm => cm.Name == "EditBusiness"))
			{
				var branchPhotos  = new Event
				{
					Name = "EditBusiness",
                    Description = "Edit Business",
                    EventCategoryID = 1
                };

				dbContext.Event.Add(branchPhotos);
				dbContext.SaveChanges();
			}



            Console.WriteLine("Completed Executing " + this.GetType().Name);



            this.updateDatabaseVersion(this.GetType().Name);
        }

        //update the same row that it is complete the running 

    }
}
