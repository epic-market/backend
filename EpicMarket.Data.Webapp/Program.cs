// See https://aka.ms/new-console-template for more information
using EpicMarket.Data.Models;
using EpicMarket.Data.Webapp;
using EpicMarket.Data.Webapp.AlterScripts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;



var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration config = builder.Build();

// Create HttpContextAccessor
var httpContextAccessor = new HttpContextAccessor();

var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

// Create DbContext with options
var dbContext = new ApplicationDbContext(
    configuration: config,
    httpContextAccessor: httpContextAccessor
);

var alterScriptVertions = new AlterScriptVersions(dbContext);

alterScriptVertions.Execute();

var listOfScriptNotExcuted = dbContext.DatabaseVersions.Where(c => c.Status == false).ToList();


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





