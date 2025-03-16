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
            AddDatabaseVersion("AlterScript5", "This is seed data adding data for ApplicationConfiguration in branches", "Gadamsetti Akhil", new AlterScript5(this.dbContext)); 
            AddDatabaseVersion("AlterScript6", "This is seed data adding data for Events, ApplicationConfiguration and ATTACHMENTTYPE", "Vinay", new AlterScript6(this.dbContext));
            AddDatabaseVersion("AlterScript7", "This is seed data adding data for ProductInternal Attachment", "Gadamsetti Akhil", new AlterScript7(this.dbContext));
            AddDatabaseVersion("AlterScript8", "This is seed data adding data for Profiles, AttachmentType and Entity", "Gadamsetti Akhil", new AlterScript8(this.dbContext));
            AddDatabaseVersion("AlterScript9", "This is seed data adding data for CatelogVariant", "Gadamsetti Akhil", new AlterScript9(this.dbContext));
            AddDatabaseVersion("AlterScript10", "This is seed data adding data for AccessControlList, ApplicationTable, ApplicationSecurable, AccessType, ApplicationConfiguration", "Gadamsetti Akhil", new AlterScript10(this.dbContext));
            AddDatabaseVersion("AlterScript11", "This is seed data adding data for AppRoles", "Gadamsetti Akhil", new AlterScript11(this.dbContext));
            AddDatabaseVersion("AlterScript12", "This is seed data adding data for AttachmentType", "Gadamsetti Akhil", new AlterScript12(this.dbContext));
            AddDatabaseVersion("AlterScript13", "This is seed data adding data for ApplicationSecurables", "Gadamsetti Akhil", new AlterScript13(this.dbContext));
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
                    CreateDate = DateTime.Now,
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
