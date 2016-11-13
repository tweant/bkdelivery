using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Interfaces
{
    public interface IAddressesRepository : IRepository
    {

        IEnumerable<Address> GetUserAddresses(int userId, int addressTypeId);
        IEnumerable<Address> GetAllUserAddresses(int userId);      
        void Add(Address address);
    }
}
