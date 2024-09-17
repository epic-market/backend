using EpicMarket.Contracts;
using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Services
{

        public class ApplicationConfigurationService : IApplicationConfigurationService
        {

            private readonly ApplicationDbContext dbContext;
            private readonly ILogger<ApplicationConfigurationService> _loggerService;


            public ApplicationConfigurationService(
                                                    ILogger<ApplicationConfigurationService> _loggerService,
                                
                                                    ApplicationDbContext dbContext
                )
            {
                this._loggerService = _loggerService;
   
                this.dbContext = dbContext;
            }


            public ApplicationConfiguration GetByID(int? id)
            {
                return dbContext.ApplicationConfigurations.Where(a => a.ID == id).FirstOrDefault();
            }


            public ApplicationConfiguration GetByName(string name)
            {
                return dbContext.ApplicationConfigurations.Where(a => a.Name == name).FirstOrDefault(); ;
            }

            public string GetApplicationConfigurationValue(string name)
            {
                string applicationConfigurationValue = string.Empty;
                var appConfig = dbContext.ApplicationConfigurations.Where(a => a.Name == name).FirstOrDefault();

                if (appConfig != null)
                {
                    applicationConfigurationValue = appConfig.Value;
                }

                return applicationConfigurationValue;
            }

    
        }

}
