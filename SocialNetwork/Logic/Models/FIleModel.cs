namespace Logic.Models
{
    public class FileModel
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public byte[] Data { get; set; }
        public long Length { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public bool IsAvatar { get; set; }

        public User User { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}