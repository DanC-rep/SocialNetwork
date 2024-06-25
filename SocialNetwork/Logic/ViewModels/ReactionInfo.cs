using Logic.Enums;

namespace Logic.ViewModels
{
    public class ReactionInfo
    {
        public int Id { get; set; }
        public ReactionType ReactionType { get; set; }
        public string Path { get; set; } = string.Empty;
        public int ReactionsCount { get; set; }
    }
}