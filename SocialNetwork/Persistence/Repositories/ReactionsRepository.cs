using Logic.Models;
using Logic.Interfaces;
using Logic.ViewModels;
using Logic.Enums;

namespace Persistence.Repositories
{
    public class ReactionsRepository : IReactionsRepository
    {
        private readonly NetworkDbContext context;

        public ReactionsRepository(NetworkDbContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<Reaction> GetReactionsList()
        {
            return context.Reactions;
        }

        public Reaction GetReactionById(int id)
        {
            return context.Reactions.Where(r => r.Id == id).SingleOrDefault();
        }

        public int GetPhotoReactionCount(FileModel photo, int reactionId)
        {
            return context.ReactionsToFiles.Where(r => r.FileId == photo.Id && r.ReactionId == reactionId).Count();
        }

        public ReactionToFile GetReactionToFile(int photoId, string userId)
        {
            return context.ReactionsToFiles.Where(r => r.FileId == photoId && r.UserId == userId).SingleOrDefault();
        }

        public void DeleteReactionToFile(ReactionToFile reactionToFile)
        {
            context.ReactionsToFiles.Remove(reactionToFile);
            context.SaveChanges();
        }

        public void AddReactionToFile(ReactionToFile reaction)
        {
            context.ReactionsToFiles.Add(reaction);
            context.SaveChanges();
        }

        public void UpdateReactionToPhoto(ReactionToFile reactionToFile, Reaction reaction)
        {
            reactionToFile.Reaction = reaction;
            context.SaveChanges();
        }
    }
}