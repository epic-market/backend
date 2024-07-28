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
    public class AlterScript3 : BaseAlterScript,IAlterScript
    {
        private readonly ApplicationDbContext context;

        public AlterScript3(ApplicationDbContext context) :base(context)
        {
            this.context = context;
        }

        public void Execute()
        {
            if (!context.ContactMethod.Any())
            {

                var contactMethod = new List<ContactMethod>
                {
                  new ContactMethod{ Name = "Email" , Description ="Email to be Sent",CreateBy="sys",CreateDate=DateTime.Now},
                };

                foreach (var accessType in contactMethod)
                {
                    context.ContactMethod.Add(accessType);
                }
                context.SaveChanges();
            }
            if (!context.PersonTypes.Any(cm => cm.Type == "Business Owner"))
            {
                var personType = new PersonType
                {
                    Type = "Business Owner",
                    Description = "Business Owner",
                };

                context.PersonTypes.Add(personType);
                context.SaveChanges();
            }
            if (!context.PersonTypes.Any(cm => cm.Type == "Business Employee"))
            {
                var personType = new PersonType
                {
                    Type = "Business Employee",
                    Description = "Business Employee",
                };

                context.PersonTypes.Add(personType);
                context.SaveChanges();
            }

            var businessOwner = context.PersonTypes
                                       .FirstOrDefault(pt => pt.Type == "Business Owner");
            if (businessOwner != null)
            {
                if (!context.SupportQuerys.Any(sq => sq.Query == "Issue with the login" && sq.TypeofPersonid == businessOwner.ID))
                {
                    var supportQuery = new SupportQuerys
                    {
                        Query = "Issue with the login",
                        TypeofPersonid = businessOwner.ID, 
                        CreateDate = DateTime.Now,
                        CreateBy = "system",
                        TaskTypeID=1
                    };

                    context.SupportQuerys.Add(supportQuery);
                    context.SaveChanges();
                }
            }

            var businessOwner2 = context.PersonTypes
                                       .FirstOrDefault(pt => pt.Type == "Business Employee");
            if (businessOwner != null)
            {
                if (!context.SupportQuerys.Any(sq => sq.Query == "Issue with the login" && sq.TypeofPersonid== businessOwner2.ID))
                {
                    var supportQuery = new SupportQuerys
                    {
                        Query = "Issue with the login",
                        TypeofPersonid = businessOwner2.ID,
                        CreateDate = DateTime.Now,
                        CreateBy = "system",
                        TaskTypeID = 1
                    };

                    context.SupportQuerys.Add(supportQuery);
                    context.SaveChanges();
                }
            }
            if (!context.AttachmentTypes.Any(cm => cm.Name == "Logo"))
            {
                var personType = new AttachmentType
                {
                    Name = "Logo",
                    Description = "Logo",
                };

                context.AttachmentTypes.Add(personType);
                context.SaveChanges();
            }
            if (!context.AttachmentTypes.Any(cm => cm.Name == "Proof"))
            {
                var personType = new AttachmentType
                {
                    Name = "Proof",
                    Description = "Proof",
                };

                context.AttachmentTypes.Add(personType);
                context.SaveChanges();
            }
            if (!context.ApplicationConfigurations.Any(cm => cm.Name == "BUSINESSPATH"))
            {
                var personType = new ApplicationConfiguration
                {
                    Name = "BUSINESSPATH",
                    Value = "BUSINESS",
                };

                context.ApplicationConfigurations.Add(personType);
                context.SaveChanges();
            }
            if (!context.ApplicationConfigurations.Any(cm => cm.Name == "BASEPATH"))
            {
                var personType = new ApplicationConfiguration
                {
                    Name = "BASEPATH",
                    Value = "EpicMarket",
                };

                context.ApplicationConfigurations.Add(personType);
                context.SaveChanges();
            }
            if (!context.ApplicationConfigurations.Any(cm => cm.Name == "LogoPATH"))
            {
                var personType = new ApplicationConfiguration
                {
                    Name = "LogoPATH",
                    Value = "Logo",
                };

                context.ApplicationConfigurations.Add(personType);
                context.SaveChanges();
            }
            if (!context.ApplicationConfigurations.Any(cm => cm.Name == "ProofPATH"))
            {
                var personType = new ApplicationConfiguration
                {
                    Name = "ProofPATH",
                    Value = "Proof",
                };

                context.ApplicationConfigurations.Add(personType);
                context.SaveChanges();
            }

            this.updateDatabaseVersion(this.GetType().Name);
        }


        //update the same row that it is complete the running 

    }
}
