using EpicMarket.Data.Models;
using EpicMarket.Admin.MVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AuthDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AuthDbContextConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AuthDbContext>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Use(async (context, next) =>
{
    // Check if the request path starts with the Identity segment
    if (context.Request.Path.StartsWithSegments("/Identity", StringComparison.OrdinalIgnoreCase))
    {
        // Paths under Identity that are allowed
        var allowedIdentityPaths = new List<string>
        {
            "/Identity/Account/Login",
            "/Identity/Account/Logout",
        };

        // Check if the request path is one of the allowed Identity paths
        bool isAllowedIdentityPath = allowedIdentityPaths.Any(path => context.Request.Path.Equals(path, StringComparison.OrdinalIgnoreCase));

        // If the request is for an allowed Identity path, proceed with the request
        if (isAllowedIdentityPath)
        {
            await next();
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
