using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKDelivery.Domain.Model
{
    public class Client
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public Address HouseAddress { get; set; }
        public Address InvoiceAddress { get; set; }
        public List<Address> DeliveryAddresses { get; set; }
        public List<Order> Orders { get; set; }

    }
}
