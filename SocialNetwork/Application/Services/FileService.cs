using Application.Options;
using Logic.Interfaces;
using Logic.Models;
using Logic.ViewModels;


namespace Application.Services
{
    public class FileService
    {
        private readonly IUserFilesRepository userFilesRepository;

        public FileService(IUserFilesRepository _usrFilesRepo)
        {
            userFilesRepository = _usrFilesRepo;
        }

        public string GetFileExtension(string fileName)
        {
            return Path.GetExtension(fileName).ToLowerInvariant();
        }

        public FileModel CreateFileModel(string fileName, long length, string mimeType, Stream stream, User user, bool isAvatar)
        {
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);

            return new FileModel
            {
                FileName = uniqueFileName,
                Length = length,
                ContentType = mimeType,
                Data = ConvertToByteArray(stream, length),
                IsAvatar = isAvatar,
                User = user
            };
        }

        public byte[] ConvertToByteArray(Stream stream, long length)
        {
            byte[] fileData;

            using (BinaryReader reader = new BinaryReader(stream))
            {
                fileData = reader.ReadBytes((int)length);
            }

            return fileData;
        }

        public async Task SaveFile(FileModel file)
        {
            if (file.IsAvatar)
            {
                DisableAvatars(file);
            }
            userFilesRepository.Add(file);
        }

        public FileModel GetUserAvatar(User user)
        {
            return userFilesRepository.GetUserAvatar(user) ?? new FileModel();
        }

        public IEnumerable<PhotoInfo> GetUserPhotosInfo(User user)
        {
            var fileModels = userFilesRepository.GetUserPhotos(user);
            return fileModels.Select(f => CreatePhotoInfo(f));
        }

        public MemoryStream OpenDefaultImage()
        {
            var memoryStream = new MemoryStream();
            var fileStream = File.OpenRead(FileSettings.DefaultImageSettings["Path"]);
            fileStream.CopyTo(memoryStream);

            return memoryStream;
        }

        public PhotoInfo CreatePhotoInfo(FileModel model)
        {
            return new PhotoInfo
            {
                Id = model.Id,
                Data = Convert.ToBase64String(model.Data)
            };
        }

        public FileModel GetById(int id)
        {
            return userFilesRepository.GetPhotoById(id);
        }

        public void DeletePhoto(int id)
        {
            var file = GetById(id);
            userFilesRepository.Delete(file);
        }

        public void ChangeUserAvatar(int id)
        {
            var file = GetById(id);
            DisableAvatars(file);
            file.IsAvatar = true;
            userFilesRepository.SetAvatar(file);
        }

        private void DisableAvatars(FileModel file)
        {
            userFilesRepository.DisableAvatars(file);
        }
    }
}