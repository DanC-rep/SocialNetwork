using Application.Options;
using Logic.Interfaces;
using Logic.Models;
using Logic.ViewModels;

namespace Application.Services
{
    public class FileService
    {
        private readonly IUserFilesRepository userFilesRepository;
        private readonly ReactionsService reactionsService;
        private readonly CommentsService commentsService;

        public FileService(IUserFilesRepository _usrFilesRepo, ReactionsService _reactionsService, 
        CommentsService _commentsService)
        {
            userFilesRepository = _usrFilesRepo;
            reactionsService = _reactionsService;
            commentsService = _commentsService;
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
                User = user,
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

        public void SaveFile(FileModel file)
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

        public async Task<IEnumerable<PhotoInfo>> GetUserPhotosInfo(User user)
        {
            var fileModels = userFilesRepository.GetUserPhotos(user);
            var photosInfo = new List<PhotoInfo>();

            foreach (var file in fileModels)
            {
                var photoInfo = await CreatePhotoInfo(file);
                photosInfo.Add(photoInfo);
            }

            return photosInfo;
        }

        public MemoryStream OpenDefaultImage()
        {
            var memoryStream = new MemoryStream();
            var fileStream = File.OpenRead(FileSettings.DefaultImageSettings["Path"]);
            fileStream.CopyTo(memoryStream);

            return memoryStream;
        }

        public async Task<PhotoInfo> CreatePhotoInfo(FileModel model)
        {
            return new PhotoInfo
            {
                Id = model.Id,
                Data = Convert.ToBase64String(model.Data),
                UserId = model.UserId,
                ReactionsInfo = reactionsService.GetPhotoReactions(model),
                CommentsInfo =  await commentsService.GetPhotoComments(model.Id)
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