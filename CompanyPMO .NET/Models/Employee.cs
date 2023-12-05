using CompanyPMO_.NET.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    [Table("employees")]
    public class Employee : IEmployeeEntity
    {
        [Column("employee_id")]
        public int EmployeeId { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("role")]
        public string Role { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("phone_number")]
        public string PhoneNumber { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("first_name")]
        public string FirstName { get; set; }
        [Column("last_name")]
        public string LastName { get; set; }
        [Column("gender")]
        public string Gender { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("profile_picture")]
        public string? ProfilePicture { get; set; }
        [Column("last_login")]
        public DateTime? LastLogin { get; set; }
        [Column("company_id")]
        public int CompanyId { get; set; }
        [Column("tier_id")]
        public int TierId { get; set; }
        [Column("lockout_enabled")]
        public bool LockedEnabled { get; set; }
        [Column("login_attempts")]
        public int LoginAttempts { get; set; }
        [Column("locked_until")]
        public DateTime? LockedUntil { get; set; }
        [Column("supervisor_id")]
        public int? SupervisorId { get; set; }

        // Navigation properties

        public Tier Tier { get; set; }
        public Company Company { get; set; }
        public List<Project>? Projects { get; set; }
        public List<Task>? Tasks { get; set; }
        public List<Issue>? Issues { get; set; }
        public Employee? Supervisor { get; set; }
        public List<Employee>? Employees { get; set; }
    }
}
