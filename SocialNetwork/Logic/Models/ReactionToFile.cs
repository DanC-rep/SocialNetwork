namespace Logic.Models
{
    public class ReactionToFile
    {
        public FileModel File { get; set; }
        public int FileId { get; set; }

        public Reaction Reaction { get; set; }
        public int ReactionId { get; set; }

        public User User { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}