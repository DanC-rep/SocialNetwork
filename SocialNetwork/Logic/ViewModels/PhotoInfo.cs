namespace Logic.ViewModels
{
    public class PhotoInfo
    {
        public int Id { get; set; }
        public string Data { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public List<ReactionInfo> ReactionsInfo { get; set; }
        public List<CommentInfo> CommentsInfo { get; set; }
    }
}