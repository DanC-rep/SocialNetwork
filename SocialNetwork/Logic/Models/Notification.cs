using Logic.Enums;

namespace Logic.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public NotificationType NotificationType { get; set; }
        public DateTime CreationDate { get; set; }
        public string Description { get; set; } = string.Empty;

        public User Sender { get; set; }
        public string SenderId { get; set; } = string.Empty;

        public User Receiver { get; set; }
        public string ReceiverId { get; set; } = string.Empty; 
    }
}