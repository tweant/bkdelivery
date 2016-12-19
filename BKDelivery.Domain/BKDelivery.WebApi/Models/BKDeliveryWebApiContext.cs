using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BKDelivery.WebApi.Models
{
    public class BKDeliveryWebApiContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public BKDeliveryWebApiContext() : base("name=BKDeliveryWebApiContext")
        {
            Database.SetInitializer<BKDeliveryWebApiContext>(new DbInitializer());
        }

        public System.Data.Entity.DbSet<BKDelivery.WebApi.Models.Client> Clients { get; set; }
        public System.Data.Entity.DbSet<BKDelivery.WebApi.Models.Courier> Couriers { get; set; }
        public System.Data.Entity.DbSet<BKDelivery.WebApi.Models.Order> Orders { get; set; }
        public System.Data.Entity.DbSet<BKDelivery.WebApi.Models.Category> Categories { get; set; }
        public System.Data.Entity.DbSet<BKDelivery.WebApi.Models.AddressType> AddressTypes { get; set; }
    }

    public class DbInitializer : DropCreateDatabaseIfModelChanges<BKDeliveryWebApiContext>
    {
        protected override void Seed(BKDeliveryWebApiContext context)
        {
            base.Seed(context);

            context.AddressTypes.Add(new AddressType { Name = "HouseAddress" });
            context.AddressTypes.Add(new AddressType { Name = "InvoiceAddress" });
            context.AddressTypes.Add(new AddressType { Name = "DeliveryAddress" });

            context.Categories.Add(new Category { Name = "Fashion", Multiplier = 31 });
            context.Categories.Add(new Category { Name = "Home & Garden", Multiplier = 39 });
            context.Categories.Add(new Category { Name = "Electronics", Multiplier = 42 });
            context.Categories.Add(new Category { Name = "Leisure", Multiplier = 36 });
            context.Categories.Add(new Category { Name = "Collectables", Multiplier = 24 });
            context.Categories.Add(new Category { Name = "Health & Beauty", Multiplier = 14 });
            context.Categories.Add(new Category { Name = "Motors", Multiplier = 57 });
        }
    }
}