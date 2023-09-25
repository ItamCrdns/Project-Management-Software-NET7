using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    public class User
    {
        [Column("user_id")]
        public int UserId { get; set; }
        public string Username { get; set; }
        [Column("first_name")]
        public string? FirstName { get; set; }
        [Column("last_name")]
        public string? LastName { get; set; }
        public string Gender { get; set; }
        public DateTimeOffset Created { get; set; }
        [Column("profile_picture")]
        public string? ProfilePicture { get; set; }
        [Column("last_login")]
        public DateTimeOffset? LastLogin { get; set; }
    }
}
