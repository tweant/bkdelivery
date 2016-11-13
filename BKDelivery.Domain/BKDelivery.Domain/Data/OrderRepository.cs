using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Interfaces;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Data
{
    public class OrderRepository : SqlRepository, IOrderRepository
    {
        public OrderRepository(IDbContext context) : base(context)
        {
        }

        public void Add(Order order)
        {
            
            this.Insert(order);
            order.Client = GetAll<Client>().First(x=>x.ClientId==order.OrderId);
            var id = order.OrderId;
        }
    }
}
