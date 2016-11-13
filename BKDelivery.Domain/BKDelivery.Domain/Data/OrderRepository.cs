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
        //public IEnumerable<Address> GetOrderAddresses(int orderId)
        //{
        //    return GetAll<Address>().Where(a => a.AddressId == orderId).SelectMany(o => o.Packages).AsEnumerable();
        //}

        public IEnumerable<Order> GetOrders(int orderId, int clientId, int courierId)
        {
            if (orderId != 0 && clientId != 0 && courierId != 0)
            {
                return this.GetAll<Order>().Where(x => x.OrderId == orderId && x.ClientId == clientId && x.CourierId == courierId).AsEnumerable();
            }          
            else if (orderId != 0 && clientId != 0)
            {
                return this.GetAll<Order>().Where(x => x.OrderId == orderId && x.ClientId == clientId).AsEnumerable();
            }
            else if (clientId != 0 && courierId != 0)
            {
                return this.GetAll<Order>().Where(x => x.ClientId == clientId && x.CourierId == courierId).AsEnumerable();
            }
            else if (orderId != 0 && courierId != 0)
            {
                return this.GetAll<Order>().Where(x => x.OrderId == orderId && x.CourierId == courierId).AsEnumerable();
            }
            else if (orderId != 0)
            {
                return this.GetAll<Order>().Where(x => x.OrderId == orderId).AsEnumerable();
            }
            else if (clientId != 0)
            {
                return this.GetAll<Order>().Where(x => x.ClientId == clientId).AsEnumerable();
            }
            else if (courierId != 0)
            {
                return this.GetAll<Order>().Where(x => x.CourierId == courierId).AsEnumerable();
            }
            else
            {
                return this.GetAll<Order>().AsEnumerable();
            }
        }

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
