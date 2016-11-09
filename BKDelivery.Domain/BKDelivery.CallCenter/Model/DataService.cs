using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Data;
using BKDelivery.Domain.Model;

namespace BKDelivery.CallCenter.Model
{
    public class DataService : IDataService
    {
        public IEnumerable<AddressType> AddressTypesAll()
        {
            UnitOfWork db = new UnitOfWork();
            var types = db.AddressTypesRepository.GetAll();
            db.SaveChanges();
            return types;
        }

        public void AddressAdd(Address address)
        {
            UnitOfWork db = new UnitOfWork();
            db.AddressesRepository.Add(address);
            db.SaveChanges();
        }

        public IEnumerable<Address> AddressessByClient(int clientId)
        {
            UnitOfWork db = new UnitOfWork();
            var collection = db.AddressesRepository.GetAllUserAddresses(clientId);
            db.SaveChanges();
            return collection;
        }

        public IEnumerable<Address> AddressessByClient(int clientId, int addressTypeId)
        {
            UnitOfWork db = new UnitOfWork();
            var collection = db.AddressesRepository.GetUserAddresses(clientId,addressTypeId);
            db.SaveChanges();
            return collection;
        }

        public void CourierAdd(Courier courier)
        {
            UnitOfWork db = new UnitOfWork();
            db.CouriersRepository.Add(courier);
            db.SaveChanges();
        }

        public void ClientAdd(Client client)
        {
            UnitOfWork db = new UnitOfWork();
            db.ClientsRepository.Add(client);
            db.SaveChanges();
        }

        public IEnumerable<Client> ClientsAll()
        {
            UnitOfWork db = new UnitOfWork();
            var collection = db.ClientsRepository.GetAll();
            db.SaveChanges();
            return collection;
        }

        public IEnumerable<Category> CategoriesAll()
        {
            UnitOfWork db = new UnitOfWork();
            var collection = db.CategoriesRepository.GetAll();
            db.SaveChanges();
            return collection;
        }

        public void PackageAdd(Package package)
        {
            UnitOfWork db = new UnitOfWork();
            db.PackagesRepository.Add(package);
            db.SaveChanges();
        }

        public IEnumerable<Package> PackagesByOrder(int orderId)
        {
            UnitOfWork db = new UnitOfWork();
            var collection = db.PackagesRepository.GetOrderPackages(orderId);
            db.SaveChanges();
            return collection;
        }

        public IEnumerable<Order> OrdersAll()
        {
            UnitOfWork db = new UnitOfWork();
            var collection = db.OrdersRepository.GetAll();
            db.SaveChanges();
            return collection;
        }
    }
}
