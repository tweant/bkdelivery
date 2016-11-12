using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKDelivery.Domain.Model
{
    public class Client
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public long NIP { get; set; }
        public int PhoneNumber { get; set; }
        public string EmailAddress { get; set; }

        public List<Address> Addresses { get; set; }
        public List<Order> Orders { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Client()
        {
            Addresses = new List<Address>();
            Orders = new List<Order>();
        }
    }
}
