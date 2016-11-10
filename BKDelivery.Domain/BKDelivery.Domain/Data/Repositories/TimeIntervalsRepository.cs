using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Data.Repositories
{
    public interface ITimeIntervalsRepository
    {
        void Add(TimeInterval entity, int courierId);
        void Edit(TimeInterval entity);
        IEnumerable<TimeInterval> GetCourierTimeIntervals(int courierId);
        TimeInterval FirstAvailable();
    }

    public class TimeIntervalsRepository : ITimeIntervalsRepository
    {
        private readonly BkDeliveryContext _db;
        private readonly IDbSet<TimeInterval> _set;

        public TimeIntervalsRepository(BkDeliveryContext db)
        {
            _db = db;
            _set = _db.TimeIntervals;
        }
        public void Add(TimeInterval entity, int courierId)
        {
            _set.Add(entity);
            _db.Couriers.Find(courierId).TimeIntervals.Add(entity);
        }

        public void Edit(TimeInterval entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<TimeInterval> GetCourierTimeIntervals(int courierId)
        {
            return _db.Couriers.Find(courierId).TimeIntervals.AsEnumerable();
        }

        public TimeInterval FirstAvailable()
        {
            return _set.First(x => x.IsTaken == false);
        }
    }
}
