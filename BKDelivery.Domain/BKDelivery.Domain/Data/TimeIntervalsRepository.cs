using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BKDelivery.Domain.Interfaces;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Data
{

    public class TimeIntervalsRepository : SqlRepository,ITimeIntervalsRepository
    {

        public IEnumerable<TimeInterval> GetCourierTimeIntervals(int courierId)
        {
            Courier firstOrDefault = this.GetAll<Courier>().FirstOrDefault(x=>x.CourierId==courierId);
            if (firstOrDefault != null)
                return firstOrDefault.TimeIntervals.AsEnumerable();
            throw new NullReferenceException("Courier with given id doesn't eqists.");
        }

        public TimeInterval FirstAvailable()
        {
            return GetAll<TimeInterval>().First(x => x.IsTaken == false);
        }

        public void AddPair(TimeInterval entity, int courierId)
        {
            this.Insert(entity);
            GetAll<Courier>().First(x=>x.CourierId==courierId).TimeIntervals.Add(entity);
        }

        public TimeIntervalsRepository(IDbContext context)
            : base(context)
        {
        }
    }
}
