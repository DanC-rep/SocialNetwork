namespace Logic.Models
{
    public class CommentPhoto
    {
        public Comment Comment { get; set; }
        public int CommentId { get; set; }

        public FileModel File { get; set; }
        public int FileId { get; set; }
    }
}