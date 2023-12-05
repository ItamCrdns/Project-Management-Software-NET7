using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    [Table("tiers")]
    public class Tier
    {
        [Column("tier_id")]
        public int TierId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("duty")]
        public string? Duty { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }
    }
}
