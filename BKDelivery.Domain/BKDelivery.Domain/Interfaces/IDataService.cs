using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Interfaces
{
    public interface IDataService
    {
        void InitializeDataBase();

        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class;
        TEntity Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        void Delete<TEntity>(TEntity entity) where TEntity : class;
        void Insert<TEntity>(TEntity entity) where TEntity : class;
        void Update<TEntity>(TEntity entity) where TEntity : class;

        IEnumerable<Address> AddressessByClient(int clientId);
        IEnumerable<Address> AddressessByClient(int clientId, int addressTypeId);

        List<Package> PackagesByOrder(int orderId);
        //List<Address> AddressesByOrder(int orderId);
        IEnumerable<Client> SearchClient(string name, long nip, int phonenumber, string email);
        IEnumerable<Order> SearchOrder(int orderId, int clientId, int courierId);

        void TimeIntervalAdd(TimeInterval interval, int courierId);
        void TimeIntervalAdd(IEnumerable<TimeInterval> intervals, int courierId);
        void Add(Package entity, Order order);
        void Add(Order order);
        void Add(Address address);

        KeyValuePair<TimeInterval,Courier> TimeIntervalFirstAvailable();
        IEnumerable<TimeInterval> TimeIntervalsByCourier(int courierId);
    }
}
