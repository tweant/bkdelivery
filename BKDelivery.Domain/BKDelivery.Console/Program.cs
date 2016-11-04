using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain;
using BKDelivery.Domain.Model;

namespace BKDelivery.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DeliveryContext>());

            Initialize();

            //var address = new Address
            //{
            //    Street = "Kasprzaka",
            //    City = "Warszawa",
            //    Country = "Polska",
            //    BuildingNumber = "31B",
            //    FlatNumber = "210",
            //    ZipCode = 97400,
            //    Voivodeship = "Mazowieckie"
            //};
            //var client = new Client
            //{
            //    Name = "Marcin",
            //    Surname = "Kraska",
            //    PhoneNumber = 123456789,
            //    EmailAddress = "mail@mail.com",
            //    HouseAddress = address,
            //    InvoiceAddress = address
            //};
            //client.DeliveryAddresses.Add(address);

            //using (var context = new DeliveryContext())
            //{
            //    context.Clients.Add(client);
            //    context.SaveChanges();
            //}
        }

        private static void Initialize()
        {
            var houseAddressType = new AddressType {Name = "HouseAddress"};
            var invoiceAddressType = new AddressType {Name = "InvoiceAddress"};
            var deliveryAddressType = new AddressType {Name = "DeliveryAddress"};

            using (var context = new DeliveryContext())
            {
                context.AddressTypes.Add(houseAddressType);
                context.AddressTypes.Add(invoiceAddressType);
                context.AddressTypes.Add(deliveryAddressType);

                context.SaveChanges();
            }
        }
    }

}