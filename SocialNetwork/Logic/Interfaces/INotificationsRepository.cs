using Logic.Enums;
using Logic.Models;

namespace Logic.Interfaces
{
    public interface INotificationsRepository
    {
        void Add(Notification notification);
        IEnumerable<Notification> GetUserNotifications(string user);
        void Delete(int id);
        void Delete(User sender, User receiver, NotificationType notificationType);
        Notification GetById(int id);
    }
}