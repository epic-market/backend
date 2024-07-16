
using EpicMarket.Data.ApplicationModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Principal;

namespace EpicMarket.Data.Models
{
    public class ApplicationDbContext:IdentityDbContext<AppUser, AppRole,int , IdentityUserClaim<int>,AppUserRole,IdentityUserLogin<int>,IdentityRoleClaim<int>,IdentityUserToken<int>>
    {
		private readonly IConfiguration _configuration;

		public ApplicationDbContext(IConfiguration configuration)
        {
			_configuration = configuration;
		}
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<BusinessCategoryInternal> BusinessCategories { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ApplicationConfiguration> ApplicationConfigurations { get; set; }
        public DbSet<AccessControlList> AccessControlLists { get; set; }
        public DbSet<AccessType> AccessTypes { get; set; }
        public DbSet<ApplicationSecurables> ApplicationSecurables { get; set; }
        public DbSet<Outlet> Outlets { get; set; }
        public DbSet<OutletPerson> OutletPeople { get; set; }

        public DbSet<OutletProduct> OutletProducts { get; set; }

        public DbSet<ProductInternal> ProductInternals { get; set; }

        public DbSet<PersonType> PersonTypes { get; set; }

        public DbSet<SupportTicket> SupportTickets { get; set; }

        public DbSet<BusinessEmployeeMap> BusinessEmployeeMaps { get; set; }

        public DbSet<FAQ> FAQs { get; set; }

        public DbSet<FAQCategory> FAQCategories { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<UserAddress> UserAddresses { get; set; }

        public DbSet<OrderStatusOptions> OrderStatusOptions { get; set; }

		public DbSet<StatusOptionSet> StatusOptionSets { get; set; }
        public DbSet<BlogCategory> BlogCategory { get; set; }
        public DbSet<ContactMethod> ContactMethod { get; set; }
        public DbSet<CommunicationQueue> CommunicationQueue { get; set; } 
        public DbSet<Entity> Entity { get; set; } 
        public DbSet<EventLog> EventLog { get; set; } 
        public DbSet<Event> Event { get; set; } 
        public DbSet<ApplicationsTable> ApplicationsTable { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }  
        public DbSet<TaskStatusType> TaskStatusTypes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<OrderTypesOptions> OrderTypesOptions { get; set; }
        public DbSet<PromotionalLeads> PromotionalLeads { get; set; }
        public DbSet<HelpItem> HelpItems { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<AttachmentLink> AttachmentLinks { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<AttachmentType> AttachmentTypes { get; set; } 
        public DbSet<SupportQuerys> SupportQuerys { get; set; }

        public DbSet<DatabaseVersion> DatabaseVersions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			var connectionString = _configuration.GetConnectionString("DefaultConnection");
			optionsBuilder.UseSqlServer(connectionString);
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserAddress>()
                    .HasOne(op => op.User)
                    .WithMany(u => u.UserAddresses)
                    .HasForeignKey(op => op.UserId)
                    .OnDelete(DeleteBehavior.Restrict);


			modelBuilder.Entity<UserAddress>()
                       .HasOne(op => op.Address)
                       .WithMany(u => u.UserAddresses)
                       .HasForeignKey(op => op.AddressId)
                       .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<OutletPerson>()
                      .HasOne(op => op.Person)
                      .WithMany(u => u.OutletPeople)
                      .HasForeignKey(op => op.PersonId)
                      .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<OutletPerson>()
                      .HasOne(op => op.Outlet)
                      .WithMany(u => u.OutletPeople)
                      .HasForeignKey(op => op.OutletId)
                      .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<BusinessEmployeeMap>()
                     .HasOne(op => op.Employee)
                     .WithMany(u => u.BusinessEmployeeMaps)
                     .HasForeignKey(op => op.EmployeeID)
                     .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BusinessEmployeeMap>()
                      .HasOne(op => op.Bussiness)
                      .WithMany(u => u.BusinessEmployees)
                      .HasForeignKey(op => op.BussinessID)
                      .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OutletProduct>()
                 .HasOne(op => op.Product)
                 .WithMany(u => u.OutletProducts)
                 .HasForeignKey(op => op.ProductID)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OutletProduct>()
                  .HasOne(op => op.Outlet)
                  .WithMany(u => u.OutletProducts)
                  .HasForeignKey(op => op.OutletID)
                  .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            modelBuilder.Entity<AppRole>()
              .HasMany(ur => ur.UserRoles)
              .WithOne(u => u.Roles)
              .HasForeignKey(ur => ur.RoleId)
              .IsRequired();

            modelBuilder.Entity<AppUser>()
                .HasMany(p => p.Businesses)
                .WithOne(b => b.Person)
                .HasForeignKey(b => b.PersonID)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Business>()
                .HasOne(b => b.BusinessCategory)
                .WithMany(bc => bc.Businesses)
                .HasForeignKey(b => b.BusinessCategoryID)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Business>()
                .HasOne(b => b.Address)
                .WithMany(a => a.Businesses)
                .HasForeignKey(b => b.AddressID)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Order>()
                .HasOne(o => o.Person)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.PersonID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                        .HasOne(od => od.Order)
                        .WithMany(o => o.OrderDetails)
                        .HasForeignKey(od => od.OrderID)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FAQ>()
                        .HasOne(c => c.Category)
                        .WithMany(fc => fc.FAQs)
                        .HasForeignKey(fk => fk.CategoryId)
                        .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Blog>()
                        .HasOne(c => c.BlogCategory)
                        .WithMany(fc => fc.Blogs)
                        .HasForeignKey(fk => fk.BlogCategoryID)
                        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
