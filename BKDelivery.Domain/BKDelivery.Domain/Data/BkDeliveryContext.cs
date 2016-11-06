using System.Data.Entity;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Data
{
    public class BkDeliveryContext : DbContext, IDbContext
    {
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<TimeInterval> TimeIntervals { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Order>()
            //    .HasRequired(o => o.From)
            //    .WithRequiredPrincipal()
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Order>()
            //    .HasRequired(o => o.To)
            //    .WithRequiredPrincipal()
            //    .WillCascadeOnDelete(true);

        }
    }
}