using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    public class Notification
    {
        [Column("notification_id")]
        public int NotificationId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTimeOffset Created { get; set; }
        [Column("sender_id")]
        public int? SenderId { get; set; }
        [Column("receiver_id")]
        public int ReceiverId { get; set; }
    }
}
