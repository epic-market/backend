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
    public class AlterScript4 : BaseAlterScript,IAlterScript
    {
        private readonly ApplicationDbContext dbContext;

        public AlterScript4(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Execute()
        {
			if (!dbContext.ApplicationConfigurations.Any(cm => cm.Name == "ProductPath"))
			{
				var personType = new ApplicationConfiguration
				{
					Name = "ProductPath",
					Value = "Products",
				};

				dbContext.ApplicationConfigurations.Add(personType);
				dbContext.SaveChanges();
			}

			if (!dbContext.AttachmentTypes.Any(cm => cm.Name == "Products"))
			{
				var personType = new AttachmentType
				{
					Name = "Products",
					Description = "Products",
				};

				dbContext.AttachmentTypes.Add(personType);
				dbContext.SaveChanges();
			}


			if (!dbContext.ApplicationConfigurations.Any(cm => cm.Name == "APIROUTE"))
			{
				var applicationConfiguration = new ApplicationConfiguration
				{
					Name = "APIROUTE",
					Value = "https://localhost:44372/",
				};

				dbContext.ApplicationConfigurations.Add(applicationConfiguration);
				dbContext.SaveChanges();
			}

			if (!dbContext.ApplicationConfigurations.Any(cm => cm.Name == "FILEURL"))
			{
				var applicationConfiguration = new ApplicationConfiguration
				{
					Name = "FILEURL",
					Value = "/api/Files/preview?key=",
				};

				dbContext.ApplicationConfigurations.Add(applicationConfiguration);
				dbContext.SaveChanges();
			}

			this.updateDatabaseVersion(this.GetType().Name);
        }

        //update the same row that it is complete the running 

    }
}
