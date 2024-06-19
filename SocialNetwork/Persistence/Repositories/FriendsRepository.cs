using Logic.Enums;
using Logic.Interfaces;
using Logic.Models;

namespace Persistence.Repositories
{
    public class FriendsRepository : IFriendsRepository
    {
        private readonly NetworkDbContext context;

        public FriendsRepository(NetworkDbContext ctx)
        {
            context = ctx;
        }

        public void Add(Friends friends)
        {
            context.Friends.Add(friends);
            context.SaveChanges();
        }

        public RelationType GetRelationType(User user1, User user2)
        {
            var friends = context.Friends.Where(f => f.UserId == user1.Id && f.FriendId == user2.Id).SingleOrDefault();

            if (friends == null)
            {
                return RelationType.None;
            }
            return friends.RelationType;
        }

        public Friends Get(User user1, User user2)
        {
            return context.Friends.Where(f => f.UserId == user1.Id && f.FriendId == user2.Id).SingleOrDefault();
        }

        public void UpdateRelation(Friends friends, RelationType relationType)
        {
            friends.RelationType = relationType;
            context.SaveChanges();
        }

        public IEnumerable<Friends> GetFriendsByRelation(string id, RelationType relationType)
        {
            return context.Friends.Where(f => f.UserId == id && f.RelationType == relationType);
        }
    }
}