using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Interfaces
{
    public interface IOrderRepository : IRepository
    {
        //IEnumerable<Address> GetOrderAddresses(int orderId);
        IEnumerable<Order> GetOrders(int orderId, int clientId, int courierId);
        void Add(Order order);
    }
}
