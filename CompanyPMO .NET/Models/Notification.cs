using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    [Table("notifications")]
    public class Notification
    {
        [Column("notification_id")]
        public int NotificationId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("sender_id")]
        public int? SenderId { get; set; }
        [Column("receiver_id")]
        public int ReceiverId { get; set; }
    }
}
