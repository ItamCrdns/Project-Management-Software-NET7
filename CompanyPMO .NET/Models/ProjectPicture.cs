using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    [Table("projectpictures")]
    public class ProjectPicture
    {
        [Column("project_picture_id")]
        public int ProjectPictureId { get; set; }
        [Column("project_id")]
        public int ProjectId { get; set; }
        [Column("image_url")]
        public string ImageUrl { get; set; }
        [Column("cloudinary_public_id")]
        public string CloudinaryPublicId { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("employee_id")]
        public int EmployeeId { get; set; }

        // Navigation properties
        public Project Project { get; set; }
        public Employee Employee { get; set; }
    }
}
