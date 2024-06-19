using Logic.Enums;
using Logic.Interfaces;
using Logic.Models;

namespace Application.Services
{
    public class FriendsService
    {
        private readonly IFriendsRepository repository;

        public FriendsService(IFriendsRepository repo)
        {
            repository = repo;
        }

        public void SendFriendRequest(User sender, User receiver)
        {
            var friends1 = repository.Get(sender, receiver);
            var friends2 = repository.Get(receiver, sender);

            if (friends1 != null && friends2 != null)
            {
                repository.UpdateRelation(friends1, RelationType.Follower);
                repository.UpdateRelation(friends2, RelationType.Following);
                return;
            }

            var follower = new Friends
            {
                User = sender,
                Friend = receiver,
                RelationType = RelationType.Follower
            };

            var following = new Friends
            {
                User = receiver,
                Friend = sender,
                RelationType = RelationType.Following
            };

            repository.Add(following);
            repository.Add(follower);
        }

        public void AddToFriends(User sender, User receiver)
        {
            var relationType = GetRelationType(receiver, sender);

            if (relationType == RelationType.Follower)
            {
                var friends1 = repository.Get(receiver, sender);
                repository.UpdateRelation(friends1, RelationType.Friend);

                var friends2 = repository.Get(sender, receiver);
                repository.UpdateRelation(friends2, RelationType.Friend);
            }
        }

        public void RemoveFromFriends(User sender, User receiver)
        {
            var relationType = GetRelationType(sender, receiver);

            if (relationType == RelationType.Friend)
            {
                var friends1 = repository.Get(sender, receiver);
                repository.UpdateRelation(friends1, RelationType.Following);

                var friends2 = repository.Get(receiver, sender);
                repository.UpdateRelation(friends2, RelationType.Follower);
            }
        }

        public void CancelSubscribtion(User sender, User receiver)
        {
            var relationType = GetRelationType(sender, receiver);

            if (relationType == RelationType.Follower)
            {
                var friends1 = repository.Get(sender, receiver);
                repository.UpdateRelation(friends1, RelationType.None);

                var friends2 = repository.Get(receiver, sender);
                repository.UpdateRelation(friends2, RelationType.None);
            }
        }

        public RelationType GetRelationType(User user1, User user2)
        {
            return repository.GetRelationType(user1, user2);
        }

        public IEnumerable<Friends> GetFriendsByRelation(string id, RelationType relationType)
        {
            return repository.GetFriendsByRelation(id, relationType);
        }
    }
}