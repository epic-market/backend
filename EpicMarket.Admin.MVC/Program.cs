using EpicMarket.Data.Models;
using EpicMarket.Admin.MVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Amazon.S3;
using EpicMarket.Admin.MVC.Contracts;
using EpicMarket.Admin.MVC.Services;
using EpicMarket.Admin.MVC.Middleware;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;


var builder = WebApplication.CreateBuilder(args);


builder.Configuration
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
	.AddEnvironmentVariables();


var connectionString = builder.Configuration.GetConnectionString("AuthDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AuthDbContextConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// Add browser refresh and hot reload services in development
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddRazorPages();
    builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
}

builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<IAttachmentService,AttachmentService>();
builder.Services.AddScoped<IApplicationConfigurationService, ApplicationConfigurationService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IEntityService, EntityService>();
builder.Services.AddScoped<IEventService, EventService>();

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddDefaultIdentity<AppUser>().AddDefaultTokenProviders().
        AddRoles<AppRole>()
    .AddEntityFrameworkStores<AuthDbContext>();


builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();

// Add before builder.Services.AddControllersWithViews()
builder.Services.AddSingleton<UrlContextService>();
builder.Services.AddScoped<IUrlContextService>(sp => sp.GetRequiredService<UrlContextService>());

var app = builder.Build();

// Configure the HTTP request pipeline.

// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add after app.UseRouting() and before app.UseAuthentication()
app.UseUrlContext();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Use(async (context, next) =>
{
          
    // Define public URLs
    var publicUrls = new List<string>
    {
        "/Identity/Account/Login",
        "/Identity/Account/Logout",
        "/Identity/Account/AccessDenied",
        "/Identity/Account/LoginWith2fa",
        "/Identity/Account/LoginWithRecoveryCode",
        
    };

    // Define URLs that require authentication
    var authenticatedUrls = new List<string>
    {
        "/Identity/Account/Manage",
        "/Identity/Account/Manage/ChangePassword",
        "/Identity/Account/Manage/TwoFactorAuthentication",
        "/Identity/Account/Manage/EnableAuthenticator",
        "/Identity/Account/Manage/ShowRecoveryCodes",
        "/Identity/Account/Manage/Disable2fa",
        "/Identity/Account/Manage/GenerateRecoveryCodes",
        "/Identity/Account/Manage/ResetAuthenticator",
        "/Identity/Account/Register"
    };

    // Check if the request path starts with the Identity segment
    if (context.Request.Path.StartsWithSegments("/Identity", StringComparison.OrdinalIgnoreCase))
    {
        // Check if the request path is one of the public URLs
        bool isPublicUrl = publicUrls.Any(path => context.Request.Path.Equals(path, StringComparison.OrdinalIgnoreCase));

        // Check if the request path is one of the authenticated URLs
        bool isAuthenticatedUrl = authenticatedUrls.Any(path => context.Request.Path.Equals(path, StringComparison.OrdinalIgnoreCase));

        if (isPublicUrl)
        {
            // If the request is for a public URL, proceed with the request
            await next();
        }
        else if (isAuthenticatedUrl)
        {
            // If the request is for an authenticated URL, check if the user is logged in
            if (context.User.Identity.IsAuthenticated)
            {
                // If the user is logged in, proceed with the request
                await next();
            }
            else
            {
                // If the user is not logged in, redirect to the login page
                context.Response.Redirect("/Identity/Account/Login");
            }
        }
        else
        {
            // If the request is for a different Identity path, redirect to the login page
            context.Response.Redirect("/Identity/Account/Login");
        }
    }
    else
    {
        // If the request does not start with the Identity segment, proceed with the request
        await next();
    }
});

app.MapRazorPages();

app.Run();
