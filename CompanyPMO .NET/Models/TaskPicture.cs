using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    [Table("taskpictures")]
    public class TaskPicture
    {
        [Column("task_picture_id")]
        public int TaskPictureId { get; set; }
        [Column("task_id")]
        public int TaskId { get; set; }
        [Column("image_url")]
        public string ImageUrl { get; set; }
        [Column("cloudinary_public_id")]
        public string CloudinaryPublicId { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("employee_id")]
        public int EmployeeId { get; set; }

        // Navigation properties

        public Task Task { get; set; }
        public Employee Employee { get; set; }
    }
}
