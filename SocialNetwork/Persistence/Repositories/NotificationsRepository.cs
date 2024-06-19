using Logic.Enums;
using Logic.Interfaces;
using Logic.Models;

namespace Persistence.Repositories
{

    public class NotificationsRepository : INotificationsRepository
    {
        private readonly NetworkDbContext context;

        public NotificationsRepository(NetworkDbContext ctx)
        {
            context = ctx;
        }

        public void Add(Notification notification)
        {
            context.Notifications.Add(notification);
            context.SaveChanges();
        }

        public IEnumerable<Notification> GetUserNotifications(string id)
        {
            return context.Notifications.Where(c => c.ReceiverId == id).AsEnumerable();
        }

        public void Delete(int id)
        {
            var notification = GetById(id);
            context.Notifications.Remove(notification);
            context.SaveChanges();
        }

        public void Delete(User sender, User receiver, NotificationType notificationType)
        {
            var notification = context.Notifications.Where(n => n.SenderId == sender.Id && n.ReceiverId == receiver.Id && n.NotificationType == notificationType).SingleOrDefault();

            if (notification != null)
            {
                context.Notifications.Remove(notification);
            }
            context.SaveChanges();
        }

        public Notification GetById(int id)
        {
            return context.Notifications.Where(c => c.Id == id).Single();
        }
    }
}