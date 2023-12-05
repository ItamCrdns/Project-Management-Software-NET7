using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    [Table("changelog")]
    public class Changelog
    {
        [Column("log_id")]
        public int LogId { get; set; }
        [Column("entity_type")]
        public string EntityType { get; set; }
        [Column("entity_id")]
        public int EntityId { get; set; }
        [Column("operation")]
        public string Operation { get; set; }
        [Column("employee_id")]
        public int EmployeeId { get; set; }
        [Column("modified")]
        public DateTime Modified { get; set; }
        [Column("old_data")]
        public string OldData { get; set; }
        [Column("new_data")]
        public string NewData { get; set; }
    }
}
