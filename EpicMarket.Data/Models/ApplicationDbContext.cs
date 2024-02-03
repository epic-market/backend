using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EpicMarket.Data.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext()
        {

        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<BusinessCategory> BusinessCategories { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json")
                                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Person>()
                .HasMany(p => p.Businesses)
                .WithOne(b => b.Person)
                .HasForeignKey(b => b.PersonID)
                .OnDelete(DeleteBehavior.Restrict);
            
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
                        .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
