using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Interfaces
{
    public interface IPackagesRepository : IRepository
    {
        IEnumerable<Package> GetOrderPackages(int orderId);
        //void Add(Package package);
    }
}
