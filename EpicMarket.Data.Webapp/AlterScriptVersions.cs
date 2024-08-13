using EpicMarket.Data.Models;
using EpicMarket.Data.Webapp.AlterScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicMarket.Data.Webapp
{
    public class AlterScriptVersions
    {
        private readonly ApplicationDbContext dbContext;
        public List<IAlterScript> ListOfClass { get; set; } = new List<IAlterScript>();
        public AlterScriptVersions(ApplicationDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }
        public void Execute()
        {
            AddDatabaseVersion("AlterScriptseedMigration", "This is for migarting the seed data", "Vinay", new AlterScriptseedMigration(this.dbContext));
            AddDatabaseVersion("AlterScript3", "This is seed data FOR ATTACHMENTTYPE AND PersonType", "Vinay", new AlterScript3(this.dbContext));
			AddDatabaseVersion("AlterScript4", "This is seed data adding data for ApplicationConfiguration in products", "Gadamsetti Akhil", new AlterScript4(this.dbContext));
		}

        public void AddDatabaseVersion(string versionClass, string description, string createBy , IAlterScript alterScriptClass)
        {
            var alterscript1 = dbContext.DatabaseVersions.FirstOrDefault(c => c.VersionClass == versionClass);
            if (alterscript1 == null)
            {
                InsertDatabaseVersion(new DatabaseVersion()
                {
                    VersionClass = versionClass,
                    Description = description,
                    CreateBy = createBy,
                    Status = false
                });
                ListOfClass.Add(alterScriptClass);
            }
            else if (alterscript1.Status == false)
            {
                ListOfClass.Add(alterScriptClass);
            }

        }

        bool InsertDatabaseVersion(DatabaseVersion databaseVersion)
        {
            dbContext.DatabaseVersions.Add(databaseVersion);
            dbContext.SaveChanges();
            return true;
        }

    }
}
