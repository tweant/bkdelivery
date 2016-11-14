using System.Linq;
using BKDelivery.Domain.Interfaces;
using BKDelivery.Domain.Model;
using System.Collections.Generic;

namespace BKDelivery.Domain.Data
{


    public class CouriersRepository : SqlRepository, ICouriersRepository
    {
        public IEnumerable<Courier> GetCourier(int? courierId)
        {
            return this.GetAll<Courier>().Where(x => x.CourierId == courierId).AsEnumerable();
        }
        public Courier SearchByTimeInterval(int intervalId)
        {
            return this.GetAll<Courier>().First(c => c.TimeIntervals.Any(t=>t.TimeIntervalId.Equals(intervalId)));
        }

        public CouriersRepository(IDbContext context)
            : base(context)
        {
        }
    }
}
