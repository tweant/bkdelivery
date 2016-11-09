using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Data.Repositories
{

    public interface IAddressTypesRepository
    {
        IEnumerable<AddressType> GetAll();
        void Add(AddressType entity);
    }

    public class AddressTypesRepository : IAddressTypesRepository
    {
        private readonly BkDeliveryContext _db;
        private readonly IDbSet<AddressType> _set;

        public AddressTypesRepository(BkDeliveryContext db)
        {
            _db = db;
            _set = _db.AddressTypes;
        }

        public IEnumerable<AddressType> GetAll()
        {
            return _set.AsEnumerable();
        }

        public void Add(AddressType entity)
        {
            _set.Add(entity);
        }
    }
}
