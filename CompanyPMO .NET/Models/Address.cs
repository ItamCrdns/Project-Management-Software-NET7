using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    [Table("addresses")]
    public class Address
    {
        [Column("address_id")]
        public int AddressId { get; set; }
        [Column("street_address")]
        public string StreetAddress { get; set; }
        [Column("city")]
        public string City { get; set; }
        [Column("state")]
        public string State { get; set; }
        [Column("postal_code")]
        public string PostalCode { get; set; }
        [Column("country")]
        public string Country { get; set; }
    }
}
