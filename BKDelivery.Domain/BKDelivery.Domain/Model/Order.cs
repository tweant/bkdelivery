using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKDelivery.Domain.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        public Address From { get; set; }
        public Address To { get; set; }
        public List<Package> Packages { get; set; }
        public Client ClientId { get; set; }
        public Courier CourierId { get; set; }
        

    }
}
