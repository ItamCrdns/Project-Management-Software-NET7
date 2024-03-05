using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    [Table("companies")]
    public class Company
    {
        [Column("company_id")]
        public int CompanyId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("ceo_user_id")]
        public int? CeoUserId { get; set; }
        [Column("address_id")]
        public int? AddressId { get; set; }
        [Column("contact_email")]
        public string? ContactEmail { get; set; }
        [Column("contact_phone_number")]
        public string? ContactPhoneNumber { get; set; }
        [Column("added_by_id")]
        public int? AddedById { get; set; }
        [Column("logo")]
        public string? Logo { get; set; }
        [Column("latest_project_creation")]
        public DateTime LatestProjectCreation { get; set; }

        // Navigation properties

        public ICollection<Image>? Images { get; set; }
        public ICollection<Project>? Projects { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
