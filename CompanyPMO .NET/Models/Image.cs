using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    [Table("images")]
    public class Image
    {
        [Column("image_id")]
        public int ImageId { get; set; }
        [Column("entity_type")]
        public string EntityType { get; set; }
        [Column("entity_id")]
        public int EntityId { get; set; }
        [Column("image_url")]
        public string ImageUrl { get; set; }
        [Column("public_id")]
        public string PublicId { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("uploader_id")]
        public int UploaderId { get; set; }

        // Navigation properties

        public Project? Project { get; set; }
        public Task? Task { get; set; }
        public Company? Company { get; set; }
    }
}
