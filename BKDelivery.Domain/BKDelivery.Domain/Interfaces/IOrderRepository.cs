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
        IEnumerable<Order> GetOrders(int orderId, int clientId, int courierId, string selectedStatus);
        void Add(Order order);
    }
}
