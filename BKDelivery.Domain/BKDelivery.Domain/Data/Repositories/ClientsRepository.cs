using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Data.Repositories
{
    public interface IClientsRepository
    {
        void Add(Client client);
        IEnumerable<Client> GetAll();
    }

    public class ClientsRepository : IClientsRepository
    {
        private readonly BkDeliveryContext _db;
        private readonly IDbSet<Client> _set;

        public ClientsRepository(BkDeliveryContext db)
        {
            _db = db;
            _set = _db.Clients;
        }
        public void Add(Client entity)
        {
            _set.Add(entity);

        }

        public IEnumerable<Client> GetAll()
        {
            return _set.AsEnumerable();
        }
    }
}
