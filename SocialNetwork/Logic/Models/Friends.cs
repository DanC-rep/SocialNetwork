using Logic.Enums;

namespace Logic.Models
{
    public class Friends
    {
       public User User { get; set; }
       public string UserId { get; set; } = string.Empty;

       public User Friend { get; set; }
       public string FriendId { get; set; } = string.Empty;

       public RelationType RelationType { get; set; }
    }
}