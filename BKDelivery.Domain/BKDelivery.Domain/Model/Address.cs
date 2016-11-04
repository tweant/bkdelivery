using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKDelivery.Domain.Model
{
    public class Address
    {
        public int AddressId { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string FlatNumber { get; set; }
        public int ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Voivodeship { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int AddressTypeId { get; set; }
        public AddressType AddressType { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
