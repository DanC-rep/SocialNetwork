using Logic.Enums;

namespace Logic.Models
{
    public class Reaction
    {
        public int Id { get; set; }
        public ReactionType ReactionType { get; set; }
        public string Path { get; set; } = string.Empty;
    }
}