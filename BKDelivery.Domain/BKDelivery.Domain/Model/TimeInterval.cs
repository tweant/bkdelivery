using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKDelivery.Domain.Model
{
    public class TimeInterval
    {
        [Key, ForeignKey("Order")]
        public int TimeIntervalId { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        public int? OrderId { get; set; }
        public virtual Order Order { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}