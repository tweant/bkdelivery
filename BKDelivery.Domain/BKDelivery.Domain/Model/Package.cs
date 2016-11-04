using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKDelivery.Domain.Model
{
    public class Package
    {
        public int PackageId { get; set; }
        public double Weight { get; set; }
        public string Type { get; set; }
        public Category CategoryId { get; set; }
        public double Cost { get; set; }
    }
}
