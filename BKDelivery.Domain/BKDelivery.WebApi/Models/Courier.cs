﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKDelivery.WebApi.Models
{
    public class Courier
    {
        public int CourierId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }

        public List<Order> Orders { get; set; }
        public List<TimeInterval> TimeIntervals { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }


        public Courier()
        {
            Orders = new List<Order>();
            TimeIntervals = new List<TimeInterval>();
        }
    }
}
