using Logic.Enums;
using Logic.Models;
using Logic.ViewModels;

namespace Logic.Interfaces
{
    public interface IReactionsRepository
    {
        IEnumerable<Reaction> GetReactionsList();
        int GetPhotoReactionCount(FileModel photo, int reactionId);
        Reaction GetReactionById(int id);
        ReactionToFile GetReactionToFile(int photoId, string userId);
        void DeleteReactionToFile(ReactionToFile reactionToFile);
        void AddReactionToFile(ReactionToFile reactionToFile);
        void UpdateReactionToPhoto(ReactionToFile reactionToFile, Reaction reaction);
    }
}