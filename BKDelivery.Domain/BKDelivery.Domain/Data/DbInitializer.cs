using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Data
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<BkDeliveryContext>
    {
        protected override void Seed(BkDeliveryContext context)
        {
            using (var ctx = new BkDeliveryContext())
            {
                //var sqlRep = new SqlRepository(ctx);
                //var devRole = new Role { Name = "Developer", Description = "Developer Role" };
                //var teamLeadRole = new Role { Name = "TeamLead", Description = "Team Leader Role" };
                //var managerRole = new Role { Name = "Manager", Description = "Manager Role" };
                //sqlRep.Insert<Role>(devRole);
                //sqlRep.Insert<Role>(teamLeadRole);
                //sqlRep.Insert<Role>(managerRole);

                //var team = new Team { Name = "Los Banditos", Description = "Los Banditos teams description" };
                //sqlRep.Insert<Team>(team);

                //var userSul = new User { Name = "Sul", Description = "Sul user description", email = "sul@email.com", Password = "123", Role = devRole, Team = team };
                //var userJames = new User { Name = "James", Description = "James user description", email = "james@email.com", Password = "123", Role = devRole, Team = team };
                //sqlRep.Insert<User>(userSul);
                //sqlRep.Insert<User>(userJames);

                //ctx.SaveChanges();
                var sqlRep = new SqlRepository(ctx);

                var houseAddressType = new AddressType {Name = "HouseAddress"};
                var invoiceAddressType = new AddressType {Name = "InvoiceAddress"};
                var deliveryAddressType = new AddressType {Name = "DeliveryAddress"};
                sqlRep.Insert(houseAddressType);
                sqlRep.Insert(invoiceAddressType);
                sqlRep.Insert(deliveryAddressType);

                sqlRep.Insert(new Category {Name = "Fashion", Multiplier = 31});
                sqlRep.Insert(new Category {Name = "Home & Garden", Multiplier = 39});
                sqlRep.Insert(new Category {Name = "Electronics", Multiplier = 42});
                sqlRep.Insert(new Category {Name = "Leisure", Multiplier = 36});
                sqlRep.Insert(new Category {Name = "Collectables", Multiplier = 24});
                sqlRep.Insert(new Category {Name = "Health & Beauty", Multiplier = 14});
                sqlRep.Insert(new Category {Name = "Motors", Multiplier = 57});
                ctx.SaveChanges();
            }
        }
    }
}