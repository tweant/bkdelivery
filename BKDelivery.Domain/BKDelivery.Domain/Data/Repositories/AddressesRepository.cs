using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Data.Repositories
{
    public interface IAddressesRepository
    {
        void Add(Address entity);
        void Delete(Address entity);
        void Update(Address entity);
        IEnumerable<Address> GetUserAddresses(int userId, int addressTypeId);
        IEnumerable<Address> GetAllUserAddresses(int userId);
    }

    public class AddressesRepository : IAddressesRepository
    {
        private readonly BkDeliveryContext _db;
        private readonly IDbSet<Address> _set;

        public AddressesRepository(BkDeliveryContext db)
        {
            _db = db;
            _set = _db.Addresses;
        }
        public void Add(Address entity)
        {
            entity.AddressType = _db.AddressTypes.Find(entity.AddressTypeId);
            //TODO CLIENT
            _set.Add(entity);

        }

        public void Delete(Address entity)
        {
            _set.Remove(entity);
        }

        public void Update(Address entity)
        {
            entity.AddressType = _db.AddressTypes.Find(entity.AddressTypeId);
            //TODO CLIENT
            _db.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<Address> GetUserAddresses(int userId, int addressTypeId)
        {
            return _set.Where(x => x.ClientId == userId && x.AddressTypeId==addressTypeId).AsEnumerable();
        }

        public IEnumerable<Address> GetAllUserAddresses(int userId)
        {
            return _set.Where(x => x.ClientId == userId).AsEnumerable();
        }
    }
}
