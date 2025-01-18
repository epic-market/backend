using EpicMarket.Data.ApplicationModels;
using EpicMarket.Data.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Principal;

namespace EpicMarket.Data.Models
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private List<int> _allowedBusinessIds;
        private List<int> _allowedBranchIds;
        private bool _isInitialized = false;

        public ApplicationDbContext(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }




        private bool IsUserAdmin => _httpContextAccessor.HttpContext?.User?.IsInRole("admin") ?? false;

        private List<int> AllowedBusinessIds
        {
            get
            {
                if (_allowedBusinessIds == null && !_isInitialized)
                {
                    _isInitialized = true; // Prevent recursive loop
                    _allowedBusinessIds = GetUserBusinessIds();
                }
                return _allowedBusinessIds ?? new List<int>();
            }
        }

        private List<int> AllowedBranchIds
        {
            get
            {
                if (_allowedBranchIds == null && !_isInitialized)
                {
                    _isInitialized = true; // Prevent recursive loop
                    _allowedBranchIds = GetUserBranchIds();
                }
                return _allowedBranchIds ?? new List<int>();
            }
        }

        public List<int> GetUserBusinessIds()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new List<int>();
            }

            try
            {
                var userId = int.Parse(currentUserId);

                // Use raw SQL to avoid recursive calls
                var connection = Database.GetDbConnection();
                var command = connection.CreateCommand();
                command.CommandText = @"
                SELECT DISTINCT BussinessID 
                FROM BusinessEmployeeMaps 
                WHERE EmployeeID = @userId
                UNION
                SELECT DISTINCT ID
                FROM Businesses
                WHERE PersonID = @userId";

                var parameter = command.CreateParameter();
                parameter.ParameterName = "@userId";
                parameter.Value = userId;
                command.Parameters.Add(parameter);

                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                var results = new List<int>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(reader.GetInt32(0));
                    }
                }

                return results;
            }
            catch
            {
                return new List<int>();
            }
        }

        public List<int> GetUserBranchIds()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new List<int>();
            }

            try
            {
                var userId = int.Parse(currentUserId);
                var businessIds = AllowedBusinessIds;

                // Use raw SQL to avoid recursive calls
                var connection = Database.GetDbConnection();
                var command = connection.CreateCommand();
                command.CommandText = @"
                SELECT DISTINCT o.ID
                FROM Outlets o
                LEFT JOIN OutletPeople op ON o.ID = op.OutletID
                WHERE op.PersonId = @userId
                OR o.BussinessID IN (SELECT value FROM STRING_SPLIT(@businessIds, ','))";

                var userIdParam = command.CreateParameter();
                userIdParam.ParameterName = "@userId";
                userIdParam.Value = userId;
                command.Parameters.Add(userIdParam);

                var businessIdsParam = command.CreateParameter();
                businessIdsParam.ParameterName = "@businessIds";
                businessIdsParam.Value = string.Join(",", businessIds);
                command.Parameters.Add(businessIdsParam);

                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                var results = new List<int>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(reader.GetInt32(0));
                    }
                }

                return results;
            }
            catch
            {
                return new List<int>();
            }
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

        public DbSet<Inventory> Inventory { get; set; }    

        public DbSet<ProductVariants> ProductVariants { get; set; }

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

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<OnboardingStep> OnboardingSteps { get; set; }
        public DbSet<UserOnboardingProgress> UserOnboardingProgresses { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<SusbcriptionStatus> SusbcriptionStatuses{ get; set; }


        public DbSet<Rating> Ratings { get; set; }

        public DbSet<MerchantBankAccount> MerchantBankAccounts { get; set; }

        public DbSet<MerchantUpiAccount> MerchantUpiAccounts { get; set; }

        public DbSet<MerchantFinance> Finances { get; set; }
        public DbSet<Proof> Proofs { get; set; }
        public DbSet<ProofType> ProofTypes { get; set; }

        public DbSet<OTPVerification> OTPVerifications { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        
 
            base.OnModelCreating(modelBuilder);

            // Business-level filters
            modelBuilder.Entity<Catalog>()
                .HasQueryFilter(p => IsUserAdmin || !AllowedBusinessIds.Any() ||
                    AllowedBusinessIds.Contains(p.BusinessID));

            modelBuilder.Entity<BusinessEmployeeMap>()
                .HasQueryFilter(p => IsUserAdmin || !AllowedBusinessIds.Any() ||
                    AllowedBusinessIds.Contains(p.BussinessID));

            modelBuilder.Entity<Business>()
                .HasQueryFilter(b => IsUserAdmin || !AllowedBusinessIds.Any() ||
                    AllowedBusinessIds.Contains(b.ID));

            // Outlet (Branch) with combined business and branch filtering
            modelBuilder.Entity<Outlet>()
                .HasQueryFilter(p => IsUserAdmin ||
                    ((!AllowedBusinessIds.Any() || AllowedBusinessIds.Contains(p.BussinessID)) &&
                    (!AllowedBranchIds.Any() || AllowedBranchIds.Contains(p.ID))));

            modelBuilder.Entity<MerchantFinance>()
                .HasQueryFilter(o => IsUserAdmin || !AllowedBranchIds.Any() ||
                    AllowedBranchIds.Contains(o.OutletID));

            // Branch-level filters
            modelBuilder.Entity<Inventory>()
                .HasQueryFilter(p => IsUserAdmin || !AllowedBranchIds.Any() ||
                    AllowedBranchIds.Contains(p.OutletID));

            modelBuilder.Entity<OutletPerson>()
                .HasQueryFilter(p => IsUserAdmin || !AllowedBranchIds.Any() ||
                    AllowedBranchIds.Contains(p.OutletId));

            modelBuilder.Entity<Order>()
                .HasQueryFilter(o => IsUserAdmin || !AllowedBranchIds.Any() ||
                    AllowedBranchIds.Contains(o.OutletID));


            modelBuilder.Entity<AppUser>()
                         .HasIndex(op => op.UserName)
                         .IsUnique();


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

            modelBuilder.Entity<Inventory>()
                 .HasOne(op => op.ProductVariants)
                 .WithMany(u => u.Inventory)
                 .HasForeignKey(op => op.ProductVariantID)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inventory>()
                  .HasOne(op => op.Outlet)
                  .WithMany(u => u.Inventory )
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


			modelBuilder.Entity<AttachmentLink>()
					.HasOne(c => c.Attachments)
					.WithMany(fc => fc.AttachmentLinks)
					.HasForeignKey(fk => fk.AttachmentID)
					.OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Catalog>()
                        .HasOne(c => c.Category)
                        .WithMany(c => c.Catalog)
                        .HasForeignKey(c => c.CategoryID)
                        .OnDelete(DeleteBehavior.Restrict);
                        //

            modelBuilder.ApplyDefaultValuesToEntities(
					typeof(TaskType),
					typeof(TaskStatusType),
					typeof(Tasks),
					typeof(SupportTicket),
					typeof(SupportQuerys),
					typeof(StatusOptionSet),
					typeof(ProductInternal),
					typeof(Outlet),
					typeof(Order),
					typeof(OrderDetail),
					typeof(HelpItem),
					typeof(FAQ),
					typeof(FAQCategory),
					typeof(EventLog),
					typeof(Event),
					typeof(Entity),
					typeof(ContactMethod),
					typeof(CommunicationQueue),
					typeof(Comment),
					typeof(Catalog),
					typeof(Business),
					typeof(BusinessCategoryInternal),
					typeof(Blog),
					typeof(BlogCategory),
					typeof(Attachment),
					typeof(AttachmentLink),
					typeof(ApplicationsTable),
					typeof(ApplicationSecurables),
					typeof(ApplicationConfiguration),
					typeof(Address),
					typeof(BusinessEmployeeMap),
					typeof(AppUser),
                    typeof(Page),
                    typeof(ProductVariants)
                );




		}
	}
}

public static class ModelBuilderExtensions
{
	public static void ApplyDefaultValuesToEntities(this ModelBuilder modelBuilder, params Type[] entityTypes)
	{
		foreach (var entityType in entityTypes)
		{
			modelBuilder.Entity(entityType)
				.Property<bool>("IsActive")
				.HasDefaultValueSql("1");

			modelBuilder.Entity(entityType)
				.Property<string>("CreateBy")
				.HasDefaultValueSql("'System'");

			modelBuilder.Entity(entityType)
				.Property<DateTime>("CreateDate")
				.HasDefaultValueSql("GETDATE()");
		}
	}
}