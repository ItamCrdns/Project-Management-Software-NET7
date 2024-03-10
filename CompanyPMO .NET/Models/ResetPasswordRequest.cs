using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    [Table("resetpasswordrequests")]
    public class ResetPasswordRequest
    {
        [Column("request_id")]
        public int RequestId { get; set; }
        [Column("request_guid")] // Apply request guids
        public Guid RequestGuid { get; set; } = Guid.NewGuid();
        [Column("employee_id")]
        public int EmployeeId { get; set; }
        [Column("token")]
        public int? Token { get; set; }
        [Column("token_expiry")]
        public DateTime? TokenExpiry { get; set; }
        public Employee Employee { get; set; }
    }
}
