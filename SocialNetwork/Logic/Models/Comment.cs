namespace Logic.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime PublishDate { get; set; }
        public User User { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}