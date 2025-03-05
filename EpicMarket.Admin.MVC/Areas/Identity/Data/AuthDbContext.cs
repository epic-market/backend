
using EpicMarket.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EpicMarket.Data.ApplicationModels;
using System.Reflection.Emit;

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
        builder.Entity<AppUserRole>()
.ToTable("AspNetUserRoles");
        builder.Entity<AppUserRole>()
           .HasKey(ur => new { ur.UserId, ur.RoleId });
        builder.Entity<AppUserRole>()
           .HasOne(ur => ur.User)
           .WithMany(u => u.UserRoles)
           .HasForeignKey(ur => ur.UserId)
           .HasPrincipalKey(u => u.Id);
        builder.Entity<AppUserRole>()
           .HasOne(ur => ur.Roles)
           .WithMany(r => r.UserRoles)
           .HasForeignKey(ur => ur.RoleId)
           .HasPrincipalKey(r => r.Id);
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

public DbSet<EpicMarket.Data.Models.ContactMethod> ContactMethod { get; set; } = default!;

public DbSet<EpicMarket.Data.Models.Attachment> Attachment { get; set; } = default!;

public DbSet<EpicMarket.Data.Models.PersonType> PersonType { get; set; } = default!;

public DbSet<EpicMarket.Data.Models.SupportQuerys> SupportQuerys { get; set; } = default!;

public DbSet<EpicMarket.Data.Models.Event> Event { get; set; } = default!;

public DbSet<EpicMarket.Data.Models.OrderStatusOptions> OrderStatusOptions { get; set; } = default!;

public DbSet<EpicMarket.Data.Models.OrderTypesOptions> OrderTypesOptions { get; set; } = default!;

public DbSet<EpicMarket.Data.Models.PromotionalLeads> PromotionalLeads { get; set; } = default!;

public DbSet<EpicMarket.Data.Models.Page> Page { get; set; } = default!;

public DbSet<EpicMarket.Data.Models.Notification> Notification { get; set; } = default!;

public DbSet<EpicMarket.Data.Models.HelpItem> HelpItem { get; set; } = default!;

public DbSet<EpicMarket.Data.Models.OnboardingStep> OnboardingStep { get; set; } = default!;

public DbSet<EpicMarket.Data.Models.UserOnboardingProgress> UserOnboardingProgress { get; set; } = default!;

public DbSet<EpicMarket.Data.Models.AttachmentLink> AttachmentLink { get; set; } = default!;


}
