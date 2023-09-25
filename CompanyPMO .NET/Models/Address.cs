using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    public class Address
    {
        [Column("address_id")]
        public int AddressId { get; set; }
        [Column("street_address")]
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [Column("postal_code")]
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
