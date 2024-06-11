using Logic.Interfaces;
using Logic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Persistence.Repositories
{
    public class UserFilesRepository : IUserFilesRepository
    {
        private readonly NetworkDbContext context;

        public UserFilesRepository(NetworkDbContext _contex)
        {
            context = _contex;
        }
        public async Task Add(FileModel file)
        {
            context.Files.Add(file);
            context.SaveChanges();
        }

        public FileModel GetUserAvatar(User user)
        {
            return context.Users.Include(u => u.Files).SingleOrDefault(u => u.Id == user.Id)
            .Files.Where(f => f.IsAvatar == true).SingleOrDefault();
        }

        public IEnumerable<FileModel> GetUserPhotos(User user)
        {
            return context.Users.Include(u => u.Files).SingleOrDefault(u => u.Id == user.Id).Files.OrderByDescending(f => f.Id);
        }

        public void DisableAvatars(FileModel file)
        {
            context.Users.Include(u => u.Files)?.SingleOrDefault(u => u.Id == (file.User == null ? file.UserId : file.User.Id))?
            .Files.Where(f => f.IsAvatar == true).ToList().ForEach(f => f.IsAvatar = false);

            context.SaveChanges();
        }

        public FileModel GetPhotoById(int id)
        {
            return context.Files.SingleOrDefault(f => f.Id == id);
        }

        public void Delete(FileModel file)
        {
            context.Files.Remove(file);
            context.SaveChanges();
        }

        public void SetAvatar(FileModel file)
        {
            FileModel dbEntry = context.Files.SingleOrDefault(f => f.Id == file.Id);

            if (dbEntry != null)
            {
                dbEntry.IsAvatar = file.IsAvatar;
            }
            
            context.SaveChanges();
        }
    }
}