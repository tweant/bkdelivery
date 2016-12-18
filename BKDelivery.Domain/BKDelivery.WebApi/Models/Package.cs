using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKDelivery.WebApi.Models
{
    public class Package
    {
        public int PackageId { get; set; }

        public double Weight { get; set; }
        public decimal Cost { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}