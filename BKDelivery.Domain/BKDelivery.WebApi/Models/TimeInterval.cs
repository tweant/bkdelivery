using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKDelivery.WebApi.Models
{
    public class TimeInterval
    {
        [Key]
        public int TimeIntervalId { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [Required]
        public bool IsTaken { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}