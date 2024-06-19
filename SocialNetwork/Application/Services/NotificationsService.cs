using Logic.Enums;
using Logic.Interfaces;
using Logic.Models;

namespace Application.Services
{
    public class NotificationsService
    {
        private readonly INotificationsRepository repository;

        public NotificationsService(INotificationsRepository repo)
        {
            repository = repo;
        }

        public void SendFriendRequest(User sender, User receiver)
        {
            var link = $"<a href=\"/Profile/ProfileInfo/{sender.Id}\">{sender.Name}</a>";
            var description = $"Пользователь {link} хочет добавить вас в друзья";

            var notification = new Notification
            {
                NotificationType = NotificationType.FriendRequest,
                CreationDate = DateTime.Now,
                Description = description,
                Sender = sender,
                Receiver = receiver
            };

            repository.Add(notification);
        }

        public void ApproveFriendRequest(User sender, User receiver)
        {
            var link = $"<a href=\"/Profile/ProfileInfo/{sender.Id}\">{sender.Name}</a>";
            var description = $"Пользователь {link} подтвердил вашу заявку в друзья";

            var notification = new Notification
            {
                NotificationType = NotificationType.FriendRequestApproved,
                CreationDate = DateTime.Now,
                Description = description,
                Sender = sender,
                Receiver = receiver
            };

            repository.Add(notification);
        }

        public IEnumerable<Notification> GetUserNotifications(string id)
        {
            return repository.GetUserNotifications(id);
        }

        public void Delete(int id)
        {
            repository.Delete(id);
        }

        public void Delete(User sender, User receiver, NotificationType notificationType)
        {
            repository.Delete(sender, receiver, notificationType);
        }

        public Notification GetById(int id)
        {
            return repository.GetById(id);
        }
    }
}