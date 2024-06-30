// See https://aka.ms/new-console-template for more information
using EpicMarket.Data.Models;
using EpicMarket.Data.Webapp;
using EpicMarket.Data.Webapp.AlterScripts;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;



var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration config = builder.Build();

var dbcontext = new ApplicationDbContext(config);

var alterScriptVertions = new AlterScriptVersions(dbcontext);

alterScriptVertions.Execute();

var listOfScriptNotExcuted = dbcontext.DatabaseVersions.Where(c => c.Status == false).ToList();


foreach (var item in listOfScriptNotExcuted)
{
    foreach (var script in alterScriptVertions.ListOfClass)
    {
        if (item.VersionClass == script.GetType().Name)
        {
            script.Execute();
        }
    }
}





