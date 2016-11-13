using System.Linq;
using BKDelivery.Domain.Interfaces;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Data
{


    public class CouriersRepository : SqlRepository, ICouriersRepository
    {
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
