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
    public class AlterScript14 : BaseAlterScript,IAlterScript
    {
        private readonly ApplicationDbContext dbContext;

        public AlterScript14(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }



        public void Execute()
        {

            Console.WriteLine("Started Executing " + this.GetType().Name);
            
            // Add communication statuses based on constants
            var communicationStatuses = new List<CommunicationStatus>
            {
                new CommunicationStatus { Name = "Queued", Description = "Communication is queued for sending" },
                new CommunicationStatus { Name = "Sent", Description = "Communication was sent successfully" },
                new CommunicationStatus { Name = "Failed", Description = "Communication sending failed" },
                new CommunicationStatus { Name = "Retrying", Description = "Communication is being retried after failure" },
            };
            
            foreach (var status in communicationStatuses)
            {
                if (!dbContext.CommunicationStatus.Any(cs => cs.Name == status.Name))
                {
                    dbContext.CommunicationStatus.Add(status);
                }
            }

            dbContext.SaveChanges();

            this.updateDatabaseVersion(this.GetType().Name);

            Console.WriteLine("Completed Executing " + this.GetType().Name);
        }

        //update the same row that it is complete the running 

    }
}
