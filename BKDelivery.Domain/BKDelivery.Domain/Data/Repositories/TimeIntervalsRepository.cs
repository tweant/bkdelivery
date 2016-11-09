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
        void Add(TimeInterval client);
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
        public void Add(TimeInterval entity)
        {
            _set.Add(entity);

        }
    }
}
