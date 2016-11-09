using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Data.Repositories;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly BkDeliveryContext _db = null;

        public UnitOfWork()
        {
            _db = new BkDeliveryContext();
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<BkDeliveryContext>());

            Initialize(this);
            this.SaveChanges();
        }

        //// Słownik będzie używany do sprawdzania instancji repozytoriów
        //public Dictionary<Type, object> Repositories = new Dictionary<Type, object>();

        //public IRepository<T> Repository<T>() where T : class
        //{
        //    // Jeżeli instancja danego repozytorium istnieje - zostanie zwrócona
        //    if (Repositories.Keys.Contains(typeof(T)) == true)
        //        return Repositories[typeof(T)] as IRepository<T>;
        //    // Jeżeli nie, zostanie utworzona nowa i dodana do słownika
        //    IRepository<T> repo = new GenericRepository<T>(_db);
        //    Repositories.Add(typeof(T), repo);
        //    return repo;
        //}

        private IAddressTypesRepository _addressTypesRepository;
        private ICouriersRepository _couriersRepository;
        private IAddressesRepository _addressesRepository;
        private ICategoriesRepository _categoriesRepository;
        private IClientsRepository _clientsRepository;
        private IOrdersRepository _ordersRepository;
        private IPackagesRepository _packagesRepository;
        private ITimeIntervalsRepository _timeIntervalsRepository;

        public IAddressTypesRepository AddressTypesRepository
            => _addressTypesRepository ?? (_addressTypesRepository = new AddressTypesRepository(_db));

        public IAddressesRepository AddressesRepository
            => _addressesRepository ?? (_addressesRepository = new AddressesRepository(_db));

        public ICouriersRepository CouriersRepository
            => _couriersRepository ?? (_couriersRepository = new CouriersRepository(_db));

        public ICategoriesRepository CategoriesRepository
            => _categoriesRepository ?? (_categoriesRepository = new CategoriesRepository(_db));

        public IClientsRepository ClientsRepository
            => _clientsRepository ?? (_clientsRepository = new ClientsRepository(_db));

        public IOrdersRepository OrdersRepository
            => _ordersRepository ?? (_ordersRepository = new OrdersRepository(_db));

        public IPackagesRepository PackagesRepository
            => _packagesRepository ?? (_packagesRepository = new PackagesRepository(_db));

        public ITimeIntervalsRepository TimeIntervalsRepository
            => _timeIntervalsRepository ?? (_timeIntervalsRepository = new TimeIntervalsRepository(_db));


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

        private static void Initialize(UnitOfWork uow)
        {
            var houseAddressType = new AddressType {Name = "HouseAddress"};
            var invoiceAddressType = new AddressType {Name = "InvoiceAddress"};
            var deliveryAddressType = new AddressType {Name = "DeliveryAddress"};

            IAddressTypesRepository addressTypeRepo = uow.AddressTypesRepository;
            if (!addressTypeRepo.GetAll().Any())
            {
                addressTypeRepo.Add(houseAddressType);
                addressTypeRepo.Add(invoiceAddressType);
                addressTypeRepo.Add(deliveryAddressType);
            }
        }
    }
}