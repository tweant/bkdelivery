using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Data.Repositories
{

    public interface IPackagesRepository
    {
        void Add(Package client,Order order);
        IEnumerable<Package> GetOrderPackages(int orderId);
    }

    public class PackagesRepository : IPackagesRepository
    {
        private readonly BkDeliveryContext _db;
        private readonly IDbSet<Package> _set;

        public PackagesRepository(BkDeliveryContext db)
        {
            _db = db;
            _set = _db.Packages;
        }
        public void Add(Package entity, Order order)
        {
            entity.Category = _db.Categories.Find(entity.CategoryId);
            _set.Add(entity);
            _db.Orders.Find(order.OrderId).Packages.Add(entity);

        }

        public IEnumerable<Package> GetOrderPackages(int orderId)
        {
            return _db.Orders.Where(o => o.OrderId == orderId).SelectMany(o => o.Packages).AsEnumerable();
        }
    }
}
