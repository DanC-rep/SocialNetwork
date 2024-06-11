namespace Application.Options
{
    public static class FileSettings
    {
        public static int MaxFileUploadSizeMb = 5;
        public static string[] PermittedExtensions = { ".jpg", ".png" };
        public static string[] PermittedMimeTypes = { "image/jpeg", "image/png" };
        public static Dictionary<string, string> DefaultImageSettings = new Dictionary<string, string>()
        {
            {"Path", "wwwroot/images/file.jpg"},
            {"Name", "file.jpg"},
            {"ContentType", "image/jpg"}
        };
    }
}