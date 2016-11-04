using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKDelivery.Domain.Model
{
    public class Courier
    {
        public int CourierId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int PhoneNumber { get; set; }
        public List<Order> Orders { get; set; }

    }
}
