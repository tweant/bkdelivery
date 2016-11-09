using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Data.Repositories
{

    public interface ICouriersRepository
    {
        void Add(Courier entity);

    }

    public class CouriersRepository : ICouriersRepository
    {
        private readonly BkDeliveryContext _db;
        private readonly IDbSet<Courier> _set;

        public CouriersRepository(BkDeliveryContext db)
        {
            _db = db;
            _set = _db.Couriers;
        }
        public void Add(Courier entity)
        {
            _set.Add(entity);
        }
        
    }
}
