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
    public class AlterScript5 : BaseAlterScript,IAlterScript
    {
        private readonly ApplicationDbContext dbContext;

        public AlterScript5(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }



        public void Execute()
        {
            Console.WriteLine("Started Executing " + this.GetType().Name);


            if (!dbContext.ApplicationConfigurations.Any(cm => cm.Name == "BranchPhotos"))
			{
				var branchPhotos  = new ApplicationConfiguration
				{
					Name = "BranchPhotos",
					Value = "BranchPhotos",
				};

				dbContext.ApplicationConfigurations.Add(branchPhotos);
				dbContext.SaveChanges();
			}

			if (!dbContext.AttachmentTypes.Any(cm => cm.Name == "BranchPhotos"))
			{
				var personType = new AttachmentType
				{
					Name = "BranchPhotos",
					Description = "BranchPhotos",
				};

				dbContext.AttachmentTypes.Add(personType);
				dbContext.SaveChanges();
			}

            if (!dbContext.ApplicationConfigurations.Any(cm => cm.Name == "BranchThumbnail"))
            {
                var applicationConfiguration = new ApplicationConfiguration
                {
                    Name = "BranchThumbnail",
                    Value = "BranchThumbnail",
                };

                dbContext.ApplicationConfigurations.Add(applicationConfiguration);
                dbContext.SaveChanges();
            }

            if (!dbContext.AttachmentTypes.Any(cm => cm.Name == "BranchThumbnail"))
			{
				var personType = new AttachmentType
				{
					Name = "BranchThumbnail",
					Description = "BranchThumbnail",
				};

				dbContext.AttachmentTypes.Add(personType);
				dbContext.SaveChanges();
			}

            this.updateDatabaseVersion(this.GetType().Name);

			Console.WriteLine("Completed Executing " + this.GetType().Name);

        }

        //update the same row that it is complete the running 

    }
}
