using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    [Table("companypictures")]
    public class CompanyPicture
    {
        [Column("company_picture_id")]
        public int CompanyPictureId { get; set; }
        [Column("company_id")]
        public int CompanyId { get; set; }
        [Column("image_url")]
        public string ImageUrl { get; set; }
        [Column("cloudinary_public_id")]
        public string CloudinaryPublicId { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }

        // Navigation properties
        public Company Company { get; set; }
    }
}
