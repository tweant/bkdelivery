using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Timestamp]
        public byte[] RowVersion { get; set; }


        public Courier()
        {
            Orders = new List<Order>();
        }
    }
}
