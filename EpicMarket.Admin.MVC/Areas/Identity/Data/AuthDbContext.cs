
using EpicMarket.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.ApplicationModels;

namespace EpicMarket.Admin.MVC.Data;

public class AuthDbContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

public DbSet<EpicMarket.Data.Models.TaskStatusType> TaskStatusType { get; set; } = default!;

public DbSet<EpicMarket.Data.Models.TaskType> TaskType { get; set; } = default!;

public DbSet<EpicMarket.Data.Models.CommunicationQueue> CommunicationQueue { get; set; } = default!;

public DbSet<EpicMarket.Data.Models.EventLog> EventLog { get; set; } = default!;

public DbSet<EpicMarket.Data.Models.Entity> Entity { get; set; } = default!;

public DbSet<EpicMarket.Data.Models.ApplicationsTable> ApplicationsTable { get; set; } = default!;

public DbSet<EpicMarket.Data.Models.BlogCategory> BlogCategory { get; set; } = default!;

public DbSet<EpicMarket.Data.ApplicationModels.AccessControlList> AccessControlList { get; set; } = default!;

public DbSet<EpicMarket.Data.ApplicationModels.AccessType> AccessType { get; set; } = default!;

public DbSet<EpicMarket.Data.Models.AttachmentType> AttachmentType { get; set; } = default!;

}
