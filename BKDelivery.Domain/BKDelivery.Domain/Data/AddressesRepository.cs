using System.Collections.Generic;
using System.Linq;
using BKDelivery.Domain.Interfaces;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Data
{
    public class AddressesRepository : SqlRepository, IAddressesRepository
    {
        public IEnumerable<Address> GetUserAddresses(int userId, int addressTypeId)
        {
            return
                this.GetAll<Address>()
                    .Where(x => x.ClientId == userId && x.AddressTypeId == addressTypeId)
                    .AsEnumerable();
        }

        public IEnumerable<Address> GetAllUserAddresses(int userId)
        {
            return this.GetAll<Address>().Where(x => x.ClientId == userId).AsEnumerable();
        }

        public void Add(Address address)
        {
            address.AddressType = GetAll<AddressType>().First(x => x.AddressTypeId == address.AddressTypeId);
            address.Client = GetAll<Client>().First(x => x.ClientId == address.ClientId);
            this.Insert(address);

        }


        public AddressesRepository(IDbContext context)
            : base(context)
        {
        }
    }
}