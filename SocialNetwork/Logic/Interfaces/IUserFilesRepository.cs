using Logic.Models;

namespace Logic.Interfaces
{
    public interface IUserFilesRepository
    {
        Task Add(FileModel file);
        FileModel GetUserAvatar(User user);
        void DisableAvatars(FileModel file);
        IEnumerable<FileModel> GetUserPhotos(User user);
        FileModel GetPhotoById(int id);
        void Delete(FileModel file);
        void SetAvatar(FileModel file);
    }
}