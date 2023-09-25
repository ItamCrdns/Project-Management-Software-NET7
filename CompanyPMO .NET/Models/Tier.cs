using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    public class Tier
    {
        [Column("tier_id")]
        public int TierId { get; set; }
        public string Name { get; set; }
        public string? Duty { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
