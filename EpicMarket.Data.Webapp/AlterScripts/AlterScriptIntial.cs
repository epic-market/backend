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
    public class AlterScriptIntial : BaseAlterScript,IAlterScript
    {
        private readonly ApplicationDbContext dbContext;

        public AlterScriptIntial(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Execute()
        {
            this.updateDatabaseVersion(this.GetType().Name);
        }

        //update the same row that it is complete the running 

    }
}
