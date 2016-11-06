using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain;
using BKDelivery.Domain.Data;
using BKDelivery.Domain.Model;

namespace BKDelivery.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<BkDeliveryContext>());

            var uow = new GenericUnitOfWork();

            Initialize(uow);

            uow.SaveChanges();
        }

        private static void Initialize(GenericUnitOfWork uow)
        {
            var houseAddressType = new AddressType {Name = "HouseAddress"};
            var invoiceAddressType = new AddressType {Name = "InvoiceAddress"};
            var deliveryAddressType = new AddressType {Name = "DeliveryAddress"};

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