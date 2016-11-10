using System.Collections.Generic;
using BKDelivery.Domain.Data;
using BKDelivery.Domain.Model;

namespace BKDelivery.CallCenter.Model
{
    public class DataService : IDataService
    {
        public IEnumerable<AddressType> AddressTypesAll()
        {
            UnitOfWork db = new UnitOfWork();
            IEnumerable<AddressType> types = db.AddressTypesRepository.GetAll();
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
            IEnumerable<Address> collection = db.AddressesRepository.GetAllUserAddresses(clientId);
            db.SaveChanges();
            return collection;
        }

        public IEnumerable<Address> AddressessByClient(int clientId, int addressTypeId)
        {
            UnitOfWork db = new UnitOfWork();
            IEnumerable<Address> collection = db.AddressesRepository.GetUserAddresses(clientId,addressTypeId);
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
            IEnumerable<Client> collection = db.ClientsRepository.GetAll();
            db.SaveChanges();
            return collection;
        }

        public IEnumerable<Category> CategoriesAll()
        {
            UnitOfWork db = new UnitOfWork();
            IEnumerable<Category> collection = db.CategoriesRepository.GetAll();
            db.SaveChanges();
            return collection;
        }

        public void PackageAdd(Package package, Order order)
        {
            UnitOfWork db = new UnitOfWork();
            db.PackagesRepository.Add(package,order);
            db.SaveChanges();
        }

        public IEnumerable<Package> PackagesByOrder(int orderId)
        {
            UnitOfWork db = new UnitOfWork();
            IEnumerable<Package> collection = db.PackagesRepository.GetOrderPackages(orderId);
            db.SaveChanges();
            return collection;
        }

        public void OrderAdd(Order order)
        {
            UnitOfWork db = new UnitOfWork();
            db.OrdersRepository.Add(order);
            db.SaveChanges();
        }

        public void OrderEdit(Order order)
        {
            UnitOfWork db = new UnitOfWork();
            db.OrdersRepository.Edit(order);
            db.SaveChanges();
        }

        public IEnumerable<Order> OrdersAll()
        {
            UnitOfWork db = new UnitOfWork();
            IEnumerable<Order> collection = db.OrdersRepository.GetAll();
            db.SaveChanges();
            return collection;
        }

        public void TimeIntervalAdd(TimeInterval interval, int courierId)
        {
            UnitOfWork db = new UnitOfWork();
            db.TimeIntervalsRepository.Add(interval,courierId);
            db.SaveChanges();
        }

        public void TimeIntervalAdd(IEnumerable<TimeInterval> intervals, int courierId)
        {
            UnitOfWork db = new UnitOfWork();
            foreach (TimeInterval interval in intervals)
            {
                db.TimeIntervalsRepository.Add(interval, courierId);
            }
            db.SaveChanges();
        }

        public void TimeIntervalEdit(TimeInterval interval)
        {
            UnitOfWork db = new UnitOfWork();
            db.TimeIntervalsRepository.Edit(interval);
            db.SaveChanges();
        }

        public KeyValuePair<TimeInterval, Courier> TimeIntervalFirstAvailable()
        {
            UnitOfWork db = new UnitOfWork();
            TimeInterval interval = db.TimeIntervalsRepository.FirstAvailable();
            Courier courier = db.CouriersRepository.SearchByTimeInterval(interval.TimeIntervalId);
            return new KeyValuePair<TimeInterval, Courier>(interval, courier);
        }

        public IEnumerable<TimeInterval> TimeIntervalsByCourier(int courierId)
        {
            UnitOfWork db = new UnitOfWork();
            IEnumerable<TimeInterval> collection = db.TimeIntervalsRepository.GetCourierTimeIntervals(courierId);
            db.SaveChanges();
            return collection;
        }
    }
}
