using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    [Table("users")]
    public class User
    {
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("first_name")]
        public string? FirstName { get; set; }
        [Column("last_name")]
        public string? LastName { get; set; }
        [Column("gender")]
        public string Gender { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("profile_picture")]
        public string? ProfilePicture { get; set; }
        [Column("last_login")]
        public DateTime? LastLogin { get; set; }
    }
}
