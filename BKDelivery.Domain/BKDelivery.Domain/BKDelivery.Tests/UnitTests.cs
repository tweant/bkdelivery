using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            contextMock.Setup(a => a.Set<AddressType>()).Returns(Mock.Of<IDbSet<AddressType>>);
            contextMock.Setup(a => a.Set<Address>()).Returns(Mock.Of<IDbSet<Address>>);
            contextMock.Setup(a => a.Set<Category>()).Returns(Mock.Of<IDbSet<Category>>);
            contextMock.Setup(a => a.Set<Client>()).Returns(Mock.Of<IDbSet<Client>>);
            contextMock.Setup(a => a.Set<Courier>()).Returns(Mock.Of<IDbSet<Courier>>);
            contextMock.Setup(a => a.Set<Order>()).Returns(Mock.Of<IDbSet<Order>>);
            contextMock.Setup(a => a.Set<Package>()).Returns(Mock.Of<IDbSet<Package>>);
            contextMock.Setup(a => a.Set<TimeInterval>()).Returns(Mock.Of<IDbSet<TimeInterval>>);

            var repoMock = new Mock<IRepository>();
            repoMock.Setup(x => x.Insert(It.IsAny<Client>()));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var container = GetMockedContainer(contextMock.Object, unitOfWorkMock.Object,repoMock.Object);
           
            //Action
            IDataService service = new DataService();
            service.SetContainer(container);
            service.Insert(new Client {Name="BASF"});

            // Assert
            unitOfWorkMock.Verify(a => a.StartTransaction(), Times.Exactly(1));
            unitOfWorkMock.Verify(a => a.CommitTransaction(), Times.Exactly(1));

            repoMock.Verify(x => x.Insert(It.Is<Client>(c => c.Name == "BASF")), Times.Once);

        }

        private IContainer GetMockedContainer(IDbContext ctx, IUnitOfWork uow,IRepository repoMock)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(ctx).As<IDbContext>();
            builder.RegisterInstance(uow).As<IUnitOfWork>();
            builder.RegisterInstance(repoMock).As<IRepository>();
            builder.RegisterInstance(new Mock<IAddressesRepository>().Object).As<IAddressesRepository>();
            builder.RegisterInstance(new Mock<ICouriersRepository>().Object).As<ICouriersRepository>();
            builder.RegisterInstance(new Mock<IOrderRepository>().Object).As<IOrderRepository>();
            builder.RegisterInstance(new Mock<IPackagesRepository>().Object).As<IPackagesRepository>();
            builder.RegisterInstance(new Mock<ITimeIntervalsRepository>().Object).As<ITimeIntervalsRepository>();
            return builder.Build();
        }
    }
}