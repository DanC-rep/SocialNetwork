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

        private void CreateNotification(string description, User sender, User receiver, NotificationType notificationType)
        {
            var notification = new Notification
            {
                NotificationType = notificationType,
                CreationDate = DateTime.Now,
                Description = description,
                Sender = sender,
                Receiver = receiver
            };

            repository.Add(notification);
        }

        public void SendFriendRequest(User sender, User receiver)
        {
            var link = $"<a href=\"/Profile/ProfileInfo/{sender.Id}\">{sender.Name}</a>";
            var description = $"Пользователь {link} хочет добавить вас в друзья";

            CreateNotification(description, sender, receiver, NotificationType.FriendRequest);
        }

        public void ApproveFriendRequest(User sender, User receiver)
        {
            var link = $"<a href=\"/Profile/ProfileInfo/{sender.Id}\">{sender.Name}</a>";
            var description = $"Пользователь {link} подтвердил вашу заявку в друзья";

            CreateNotification(description, sender, receiver, NotificationType.FriendRequestApproved);
        }

        public void MakeReactionToPhoto(User sender, User receiver, FileModel photo, string reaction)
        {
            var usrLink = $"<a href=\"/Profile/ProfileInfo/{sender.Id}\">{sender.Name}</a>";
            var photoLink = $"<a href=\"/Photo/PhotoInfo?photoId={photo.Id}&userId={receiver.Id}\">фото</a>";

            var description = $"Пользователь {usrLink} поставил реакцию \"{reaction}\" на ваше {photoLink}";

            CreateNotification(description, sender, receiver, NotificationType.PhotoReaction);
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