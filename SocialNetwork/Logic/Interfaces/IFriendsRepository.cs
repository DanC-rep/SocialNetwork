using Logic.Enums;
using Logic.Models;

namespace Logic.Interfaces
{
    public interface IFriendsRepository
    {
        void Add(Friends friends);
        RelationType GetRelationType(User user1, User user2);
        Friends Get(User user1, User user2);
        void UpdateRelation(Friends friends, RelationType relationType);
        IEnumerable<Friends> GetFriendsByRelation(string id, RelationType relationType);
    }
}