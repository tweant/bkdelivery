using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Interfaces
{
    public interface IClientsRepository : IRepository
    {
        IEnumerable<Client> GetClients(string name, long nip, int phonenumber, string email);
        IEnumerable<Client> GetClient(int? clientId);   
    }
}
