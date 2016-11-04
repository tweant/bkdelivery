using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKDelivery.Domain.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        public int FromAddressId { get; set; }
        public Address FromAddress { get; set; }

        public int ToAddressId { get; set; }
        public Address ToAddress { get; set; }


        public List<Package> Packages { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int CourierId { get; set; }
        public Courier Courier { get; set; }

        public int TimeIntervalId { get; set; }
        public TimeInterval TimeInterval { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Order()
        {
            Packages = new List<Package>();
        }

    }
}
