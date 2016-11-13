using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKDelivery.Domain.Model
{
    public class Category
    {
        public int CategoryId { get; set; }
        public virtual string Name { get; set; }
        public virtual double Multiplier { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
