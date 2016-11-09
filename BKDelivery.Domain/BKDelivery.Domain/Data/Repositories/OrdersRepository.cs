using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Data.Repositories
{
    public interface IOrdersRepository
    {
        void Add(Order client);
        IEnumerable<Order> GetAll();
    }

    public class OrdersRepository : IOrdersRepository
    {
        private readonly BkDeliveryContext _db;
        private readonly IDbSet<Order> _set;

        public OrdersRepository(BkDeliveryContext db)
        {
            _db = db;
            _set = _db.Orders;
        }
        public void Add(Order entity)
        {
            entity.Client = _db.Clients.Find(entity.ClientId);
            _set.Add(entity);

        }

        public IEnumerable<Order> GetAll()
        {
            return _set.AsEnumerable();
        }
    }
}
