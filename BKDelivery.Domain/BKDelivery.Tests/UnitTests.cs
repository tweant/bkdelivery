using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Autofac;
using BKDelivery.Domain.Data;
using BKDelivery.Domain.Interfaces;
using BKDelivery.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BKDelivery.Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void RunAndCommitTransaction_WithDefault()
        {
            // Arrange
            var contextMock = new Mock<IDbContext>();

            var repoMock = new Mock<IRepository>();
            repoMock.Setup(x => x.Insert(It.IsAny<Client>()));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var container = GetMockedContainer(contextMock.Object, unitOfWorkMock.Object, repoMock.Object, null, null,
                null, null, null);

            //Action
            IDataService service = new DataService();
            service.SetContainer(container);
            service.Insert(new Client {Name = "BASF"});

            // Assert
            unitOfWorkMock.Verify(a => a.StartTransaction(), Times.Exactly(1));
            unitOfWorkMock.Verify(a => a.CommitTransaction(), Times.Exactly(1));

            repoMock.Verify(x => x.Insert(It.Is<Client>(c => c.Name == "BASF")), Times.Once);
        }

        [TestMethod]
        public void FromOrderToUser()
        {
            // Arrange
            var contextMock = new Mock<IDbContext>();
            contextMock.Setup(a => a.Set<AddressType>()).Returns(Mock.Of<IDbSet<AddressType>>);
            contextMock.Setup(a => a.Set<Address>()).Returns(Mock.Of<IDbSet<Address>>);
            contextMock.Setup(a => a.Set<Category>()).Returns(Mock.Of<IDbSet<Category>>);
            contextMock.Setup(a => a.Set<Client>()).Returns(Mock.Of<IDbSet<Client>>);
            contextMock.Setup(a => a.Set<Courier>()).Returns(Mock.Of<IDbSet<Courier>>);
            contextMock.Setup(a => a.Set<Order>()).Returns(Mock.Of<IDbSet<Order>>);
            contextMock.Setup(a => a.Set<Package>()).Returns(Mock.Of<IDbSet<Package>>);
            contextMock.Setup(a => a.Set<TimeInterval>()).Returns(Mock.Of<IDbSet<TimeInterval>>);

            var repoMock = new Mock<IRepository>();
            var addressesrepoMock = new Mock<IAddressesRepository>();
            var couriersRepoMock = new Mock<ICouriersRepository>();
            var ordersRepoMock = new Mock<IOrderRepository>();
            var packagesRepoMock = new Mock<IPackagesRepository>();
            var timeintervalRepoMock = new Mock<ITimeIntervalsRepository>();
            repoMock.Setup(x => x.Insert(It.IsAny<Client>()));
            timeintervalRepoMock.Setup(x => x.AddPair(It.IsAny<TimeInterval>(),It.IsAny<int>()));
            repoMock.Setup(x => x.Insert(It.IsAny<Courier>()));
            addressesrepoMock.Setup(x => x.Add(It.IsAny<Address>()));
            ordersRepoMock.Setup(x => x.Add(It.IsAny<Order>()));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var container = GetMockedContainer(contextMock.Object, unitOfWorkMock.Object, repoMock.Object, addressesrepoMock.Object, couriersRepoMock.Object,
                ordersRepoMock.Object, packagesRepoMock.Object, timeintervalRepoMock.Object);

            //Action
            IDataService service = new DataService();
            service.SetContainer(container);
            Courier courier = new Courier
            {
                Name = "Jan",
                Surname = "Kochanowski",
                PhoneNumber = 111222333,
                CourierId = 1,
            };
            var timeintervalCollection = new List<TimeInterval>
            {
                new TimeInterval
                {
                    IsTaken = false,
                    Start = new DateTime(2016, 11, 15, 12, 0, 0),
                    End = new DateTime(2016, 11, 16, 12, 0, 0),
                    TimeIntervalId = 1,
                },
                new TimeInterval
                {
                    IsTaken = false,
                    Start = new DateTime(2016, 11, 16, 12, 0, 0),
                    End = new DateTime(2016, 11, 17, 12, 0, 0),
                    TimeIntervalId = 2,
                }
            };
            service.Insert(courier);
            service.TimeIntervalAdd(timeintervalCollection, courier.CourierId);
            timeintervalRepoMock.Setup(x => x.FirstAvailable()).Returns(()=>timeintervalCollection.ElementAt(0));
            Client client = new Client
            {
                Name = "TESLA",
                NIP = 1541025154,
                PhoneNumber = 111555444,
                EmailAddress = "tesla@wp.pl",
                ClientId=1,
            };
            service.Insert(client);

            Address homeAddress = new Domain.Model.Address
            {
                ClientId = client.ClientId,
                ZipCode = 10500,
                City = "Lodz",
                BuildingNumber = "15",
                Country = "Poland",
                Street = "Wielka",
                Voivodeship = "łódzkie",
                AddressTypeId = 1,
                AddressId=1,
            };
            Address invoiceAddress = new Domain.Model.Address
            {
                ClientId = client.ClientId,
                ZipCode = 10500,
                City = "Lodz",
                BuildingNumber = "15",
                Country = "Poland",
                Street = "Wielka",
                Voivodeship = "łódzkie",
                AddressTypeId = 2,
                AddressId=2,
            };
            Address deliveryAddress = new Domain.Model.Address
            {
                ClientId = client.ClientId,
                ZipCode = 10500,
                City = "Lodz",
                BuildingNumber = "15",
                Country = "Poland",
                Street = "Wielka",
                Voivodeship = "łódzkie",
                AddressTypeId = 3,
                AddressId=3,
            };
            service.Add(homeAddress);
            service.Add(invoiceAddress);
            service.Add(deliveryAddress);
            Order order = new Order
            {
                CourierId = courier.CourierId,
                ClientId = client.ClientId,
                FromAddressId = homeAddress.AddressId,
                InvoiceAddressId = invoiceAddress.AddressId,
                ToAddressId = deliveryAddress.AddressId,
                TimeIntervalId = timeintervalCollection.ElementAt(1).TimeIntervalId,
            };
            service.Add(order);
            var firstTimeinterval = service.TimeIntervalFirstAvailable();

            // Assert
            repoMock.Verify(x => x.Insert(client),Times.Once);
            timeintervalRepoMock.Verify(x => x.AddPair(It.IsAny<TimeInterval>(), It.IsAny<int>()), Times.Exactly(2));
            repoMock.Verify(x => x.Insert(courier),Times.Once);
            addressesrepoMock.Verify(x => x.Add(It.IsAny<Address>()),Times.Exactly(3));
            repoMock.Verify(x => x.Insert(order),Times.Once);
            timeintervalRepoMock.Verify(x => x.FirstAvailable());
        }

        private IContainer GetMockedContainer(IDbContext ctx, IUnitOfWork uow, IRepository repoMock,
            IAddressesRepository addressMock, ICouriersRepository couriersMock, IOrderRepository ordersMock,
            IPackagesRepository packagesMock, ITimeIntervalsRepository timeIntervalsMock)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(ctx).As<IDbContext>();
            builder.RegisterInstance(uow).As<IUnitOfWork>();
            builder.RegisterInstance(repoMock ?? new Mock<IRepository>().Object).As<IRepository>();
            builder.RegisterInstance(addressMock ?? new Mock<IAddressesRepository>().Object).As<IAddressesRepository>();
            builder.RegisterInstance(couriersMock ?? new Mock<ICouriersRepository>().Object).As<ICouriersRepository>();
            builder.RegisterInstance(ordersMock ?? new Mock<IOrderRepository>().Object).As<IOrderRepository>();
            builder.RegisterInstance(packagesMock ?? new Mock<IPackagesRepository>().Object).As<IPackagesRepository>();
            builder.RegisterInstance(timeIntervalsMock ?? new Mock<ITimeIntervalsRepository>().Object)
                .As<ITimeIntervalsRepository>();
            return builder.Build();
        }
    }
}