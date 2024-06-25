namespace Logic.ViewModels
{
    public class CommentInfo
    {
        public string CommentText { get; set; } = string.Empty;
        public DateTime PublishDate { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserSurname { get; set; } = string.Empty;
    }
}