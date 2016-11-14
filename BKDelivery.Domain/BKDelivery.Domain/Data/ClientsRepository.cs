using BKDelivery.Domain.Interfaces;
using BKDelivery.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKDelivery.Domain.Data
{
    public class ClientsRepository : SqlRepository, IClientsRepository
    {
        public IEnumerable<Client> GetClient(int? clientId)
        {
            return this.GetAll<Client>().Where(x => x.ClientId == clientId).AsEnumerable();
        }
        public IEnumerable<Client> GetClients(string name, long nip, int phonenumber, string email)
        {
            if (name != null && nip != 0 && phonenumber != 0 && email != null)
            {
                return this.GetAll<Client>().Where(x => x.Name == name && x.NIP == nip && x.PhoneNumber == phonenumber && x.EmailAddress == email).AsEnumerable();
            }
            else if (name != null && nip != 0 && phonenumber != 0)
            {
                return this.GetAll<Client>().Where(x => x.Name == name && x.NIP == nip && x.PhoneNumber == phonenumber).AsEnumerable();
            }
            else if (name != null && nip != 0 && email != null)
            {
                return this.GetAll<Client>().Where(x => x.Name == name && x.NIP == nip && x.EmailAddress == email).AsEnumerable();
            }
            else if (name != null && phonenumber != 0 && email != null)
            {
                return this.GetAll<Client>().Where(x => x.Name == name && x.PhoneNumber == phonenumber && x.EmailAddress == email).AsEnumerable();
            }
            else if (nip != 0 && phonenumber != 0 && email != null)
            {
                return this.GetAll<Client>().Where(x => x.NIP == nip && x.PhoneNumber == phonenumber && x.EmailAddress == email).AsEnumerable();
            }
            else if (name != null && nip != 0)
            {
                return this.GetAll<Client>().Where(x => x.Name == name && x.NIP == nip).AsEnumerable();
            }
            else if (name != null && phonenumber != 0)
            {
                return this.GetAll<Client>().Where(x => x.Name == name && x.PhoneNumber == phonenumber).AsEnumerable();
            }
            else if (name != null && email != null)
            {
                return this.GetAll<Client>().Where(x => x.Name == name && x.EmailAddress == email).AsEnumerable();
            }
            else if (nip != 0 && phonenumber != 0)
            {
                return this.GetAll<Client>().Where(x => x.NIP == nip && x.PhoneNumber == phonenumber).AsEnumerable();
            }
            else if (nip != 0 && email != null)
            {
                return this.GetAll<Client>().Where(x => x.NIP == nip && x.EmailAddress == email).AsEnumerable();
            }
            else if (phonenumber != 0 && email != null)
            {
                return this.GetAll<Client>().Where(x => x.PhoneNumber == phonenumber && x.EmailAddress == email).AsEnumerable();
            }
            else if (email != null)
            {
                return this.GetAll<Client>().Where(x => x.EmailAddress == email).AsEnumerable();
            }
            else if (phonenumber != 0)
            {
                return this.GetAll<Client>().Where(x => x.PhoneNumber == phonenumber).AsEnumerable();
            }
            else if (nip != 0)
            {
                return this.GetAll<Client>().Where(x => x.NIP == nip).AsEnumerable();
            }
            else if (name != null)
            {
                return this.GetAll<Client>().Where(x => x.Name == name).AsEnumerable();
            }
            else
            {
                return this.GetAll<Client>().AsEnumerable();
            }
        }

        public ClientsRepository(IDbContext context)
            : base(context)
        {
        }
    }
}
