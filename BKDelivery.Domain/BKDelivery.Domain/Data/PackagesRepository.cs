using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BKDelivery.Domain.Interfaces;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Data
{

    public class PackagesRepository : SqlRepository,IPackagesRepository
    {

        public IEnumerable<Package> GetOrderPackages(int orderId)
        {
            return GetAll<Order>().Where(o => o.OrderId == orderId).SelectMany(o => o.Packages).AsEnumerable();
        }

        public PackagesRepository(IDbContext context) : base(context)
        {
        }
    }
}
