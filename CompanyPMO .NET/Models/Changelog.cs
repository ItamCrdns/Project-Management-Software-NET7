using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    public class Changelog
    {
        [Column("log_id")]
        public int LogId { get; set; }
        [Column("entity_type")]
        public string EntityType { get; set; }
        [Column("entity_id")]
        public int EntityId { get; set; }
        public string Operation { get; set; }
        [Column("employee_id")]
        public int EmployeeId { get; set; }
        public DateTimeOffset Modified { get; set; }
        [Column("old_data")]
        public string OldData { get; set; }
        [Column("new_data")]
        public string NewData { get; set; }
    }
}
