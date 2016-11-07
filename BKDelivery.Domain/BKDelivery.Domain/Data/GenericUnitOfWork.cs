using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Data
{
    public class GenericUnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly BkDeliveryContext _db = null;
        public GenericUnitOfWork()
        {
            _db = new BkDeliveryContext();
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<BkDeliveryContext>());

            Initialize(this);
            this.SaveChanges();
        }

        // Słownik będzie używany do sprawdzania instancji repozytoriów
        public Dictionary<Type, object> Repositories = new Dictionary<Type, object>();

        public IRepository<T> Repository<T>() where T : class
        {
            // Jeżeli instancja danego repozytorium istnieje - zostanie zwrócona
            if (Repositories.Keys.Contains(typeof(T)) == true)
                return Repositories[typeof(T)] as IRepository<T>;
            // Jeżeli nie, zostanie utworzona nowa i dodana do słownika
            IRepository<T> repo = new GenericRepository<T>(_db);
            Repositories.Add(typeof(T), repo);
            return repo;
        }
        public void SaveChanges()
        {
            _db.SaveChanges();
        }
        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                    _db.Dispose();
            }
            this._disposed = true;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private static void Initialize(GenericUnitOfWork uow)
        {
            var houseAddressType = new AddressType { Name = "HouseAddress" };
            var invoiceAddressType = new AddressType { Name = "InvoiceAddress" };
            var deliveryAddressType = new AddressType { Name = "DeliveryAddress" };

            IRepository<AddressType> addressTypeRepo = uow.Repository<AddressType>();
            if (!addressTypeRepo.GetOverview().Any())
            {
                addressTypeRepo.Add(houseAddressType);
                addressTypeRepo.Add(invoiceAddressType);
                addressTypeRepo.Add(deliveryAddressType);
            }

        }
    }
}
