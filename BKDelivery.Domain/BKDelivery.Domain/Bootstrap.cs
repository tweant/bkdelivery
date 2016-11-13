using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BKDelivery.Domain.Data;
using BKDelivery.Domain.Interfaces;

namespace BKDelivery.Domain
{
    public class BootStrap
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<BkDeliveryContext>().As<IDbContext>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<SqlRepository>().As<IRepository>();
            builder.RegisterType<AddressesRepository>().As<IAddressesRepository>();
            builder.RegisterType<CouriersRepository>().As<ICouriersRepository>();
            builder.RegisterType<PackagesRepository>().As<IPackagesRepository>();
            builder.RegisterType<TimeIntervalsRepository>().As<ITimeIntervalsRepository>();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>();
            builder.RegisterType<ClientsRepository>().As<IClientsRepository>();

            return builder.Build();
        }
    }
}
