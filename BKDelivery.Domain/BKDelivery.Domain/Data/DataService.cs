using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Autofac;
using BKDelivery.Domain.Interfaces;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Data
{
    public class DataService : IDataService
    {
        private IContainer _container;

        public void SetContainer(IContainer mockedContainer)
        {
            _container = mockedContainer;
        }

        public DataService()
        {
            if (_container == null)
            {
                _container = BootStrap.BuildContainer();
            }
        }

        public void InitializeDataBase()
        {
            using (var ctx = new BkDeliveryContext())
            {
                var dummyUser = ctx.Set<Address>().FirstOrDefault();
            }
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            List<TEntity> res;
            using (var repo = _container.Resolve<IRepository>())
            {
                res = repo.GetAll<TEntity>().ToList();
            }
            return res;
        }

        public TEntity Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            TEntity res;
            using (var repo = _container.Resolve<IRepository>())
            {
                res = repo.Get(predicate);
            }
            return res;
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            using (var uof = _container.Resolve<IUnitOfWork>())
            using (var repo = _container.Resolve<IRepository>())
            {
                uof.StartTransaction();
                repo.Delete(entity);
                repo.SaveChanges();
                uof.CommitTransaction();
            }
        }

        public void Insert<TEntity>(TEntity entity) where TEntity : class
        {
            using (var uof = _container.Resolve<IUnitOfWork>())
            using (var repo = _container.Resolve<IRepository>())
            {
                uof.StartTransaction();
                repo.Insert(entity);
                repo.SaveChanges();
                uof.CommitTransaction();
            }
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            using (var uof = _container.Resolve<IUnitOfWork>())
            using (var repo = _container.Resolve<IRepository>())
            {
                uof.StartTransaction();
                repo.Update(entity);
                repo.SaveChanges();
                uof.CommitTransaction();
            }
        }

        public IEnumerable<Address> AddressessByClient(int clientId)
        {
            List<Address> res;
            using (var repo = _container.Resolve<IAddressesRepository>())
            {
                res = repo.GetAllUserAddresses(clientId).ToList();
            }
            return res;
        }

        public IEnumerable<Address> AddressessByClient(int clientId, int addressTypeId)
        {
            List<Address> res;
            using (var repo = _container.Resolve<IAddressesRepository>())
            {
                res = repo.GetUserAddresses(clientId, addressTypeId).ToList();
            }
            return res;
        }

        public List<Package> PackagesByOrder(int orderId)
        {
            List<Package> res;
            using (var repo = _container.Resolve<IPackagesRepository>())
            {
                res = repo.GetOrderPackages(orderId).ToList();
            }
            return res;
        }

        //public List<Address> AddressesByOrder(int orderId)
        //{
        //    List<Address> res;
        //    using (var repo = _container.Resolve<IOrderRepository>())
        //    {
        //        res = repo.GetOrderAddresses(orderId).ToList();
        //    }
        //    return res;
        //}

        public IEnumerable<Client> SearchClient(string name, long nip, int phonenumber, string email)
        {
            List<Client> res;
            using (var repo = _container.Resolve<IClientsRepository>())
            {
                res = repo.GetClients(name, nip, phonenumber, email).ToList();
            }
            return res;
        }

        public IEnumerable<Order> SearchOrder(int orderId, int clientId, int courierId)
        {
            List<Order> res;
            using (var repo = _container.Resolve<IOrderRepository>())
            {
                res = repo.GetOrders(orderId, clientId, courierId).ToList();
            }
            return res;
        }

        public void TimeIntervalAdd(TimeInterval interval, int courierId)
        {
            using (var uof = _container.Resolve<IUnitOfWork>())
            using (var repo = _container.Resolve<ITimeIntervalsRepository>())
            {
                uof.StartTransaction();
                repo.AddPair(interval, courierId);
                repo.SaveChanges();
                uof.CommitTransaction();
            }
        }

        public void TimeIntervalAdd(IEnumerable<TimeInterval> intervals, int courierId)
        {
            using (var uof = _container.Resolve<IUnitOfWork>())
            using (var repo = _container.Resolve<ITimeIntervalsRepository>())
            {
                uof.StartTransaction();
                foreach (TimeInterval interval in intervals)
                {
                    repo.AddPair(interval, courierId);
                }
                repo.SaveChanges();
                uof.CommitTransaction();
            }
        }

        public void Add(Package entity, Order order)
        {
            using (var uof = _container.Resolve<IUnitOfWork>())
            using (var repo = _container.Resolve<IRepository>())
            {
                uof.StartTransaction();
                repo.Insert(entity);
                Order ord = (from u in this.GetAll<Order>()
                    where u.OrderId == order.OrderId
                    select u).First();
                ord.Packages.Add(entity);
                repo.Update(ord);
                repo.SaveChanges();
                uof.CommitTransaction();
            }
        }

        public void Add(Order order)
        {
            using (var uof = _container.Resolve<IUnitOfWork>())
            using (var repo = _container.Resolve<IRepository>())
            {
                uof.StartTransaction();

                //repo.SaveChanges();
                //order.Client = GetAll<Client>().First(x => x.ClientId == order.ClientId);
                repo.Insert(order);
                //repo.Update(order);
                repo.SaveChanges();
                uof.CommitTransaction();
            }
        }

        public void Add(Address address)
        {
            using (var uof = _container.Resolve<IUnitOfWork>())
            using (var repo = _container.Resolve<IAddressesRepository>())
            {
                uof.StartTransaction();
                repo.Add(address);
                repo.SaveChanges();
                uof.CommitTransaction();
            }
        }

        public KeyValuePair<TimeInterval, Courier> TimeIntervalFirstAvailable()
        {
            TimeInterval interval;
            Courier courier;
            using (var repo = _container.Resolve<ITimeIntervalsRepository>())
            {
                interval = repo.FirstAvailable();
            }
            using (var repo = _container.Resolve<ICouriersRepository>())
            {
                courier = repo.SearchByTimeInterval(interval.TimeIntervalId);
            }
            return new KeyValuePair<TimeInterval, Courier>(interval, courier);
        }

        public IEnumerable<TimeInterval> TimeIntervalsByCourier(int courierId)
        {
            List<TimeInterval> res;
            using (var repo = _container.Resolve<ITimeIntervalsRepository>())
            {
                res = repo.GetCourierTimeIntervals(courierId).ToList();
            }
            return res;
        }
    }
}