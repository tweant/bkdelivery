using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using BKDelivery.Domain.Interfaces;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Data
{
    public class BkDeliveryContext : DbContext, IDbContext
    {

        public BkDeliveryContext() : base("name=BKDeliveryDatabase")
        {
            Database.SetInitializer(new DbInitializer());
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<AddressType>();
            modelBuilder.Entity<Address>();
            modelBuilder.Entity<Category>();
            modelBuilder.Entity<Client>();
            modelBuilder.Entity<Courier>();
            modelBuilder.Entity<Order>();
            modelBuilder.Entity<Package>();
            modelBuilder.Entity<TimeInterval>();

            base.OnModelCreating(modelBuilder);
        }
    }
}