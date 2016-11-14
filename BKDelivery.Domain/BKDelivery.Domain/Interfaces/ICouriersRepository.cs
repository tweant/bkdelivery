using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Interfaces
{
    public interface ICouriersRepository : IRepository
    {
        IEnumerable<Courier> GetCourier(int? courierId);
        Courier SearchByTimeInterval(int intervalId);
    }
}
