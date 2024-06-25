using Logic.Models;
using Logic.Interfaces;
using Logic.Enums;
using Logic.ViewModels;

namespace Application.Services
{
    public class ReactionsService
    {
        private readonly IReactionsRepository reactionsRepositoy;

        public ReactionsService(IReactionsRepository _reactionsRepository)
        {
            reactionsRepositoy = _reactionsRepository;
        }

        public IEnumerable<Reaction> GetReactionsList()
        {
            return reactionsRepositoy.GetReactionsList();
        }

        public Reaction GetReactionById(int id)
        {
            return reactionsRepositoy.GetReactionById(id);
        }

        public List<ReactionInfo> GetPhotoReactions(FileModel photo)
        {
            var reactionsList = reactionsRepositoy.GetReactionsList();
            List<ReactionInfo> reactionsInfo = new List<ReactionInfo>();

            foreach (var reaction in reactionsList)
            {
                int count = reactionsRepositoy.GetPhotoReactionCount(photo, reaction.Id);

                var reactionInfo = new ReactionInfo
                {
                    Id = reaction.Id,
                    ReactionType = reaction.ReactionType,
                    Path = reaction.Path,
                    ReactionsCount = count
                };

                reactionsInfo.Add(reactionInfo);
            }

            return reactionsInfo;
        }

        public bool MakeReaction(FileModel photo, Reaction reaction, User user)
        {
            var reactionToPhoto = reactionsRepositoy.GetReactionToFile(photo.Id, user.Id);

            if (reactionToPhoto != null)
            {
                if (reactionToPhoto.ReactionId == reaction.Id)
                {
                    reactionsRepositoy.DeleteReactionToFile(reactionToPhoto);
                    return false;
                }
                reactionsRepositoy.UpdateReactionToPhoto(reactionToPhoto, reaction);
                return true;
            }

            ReactionToFile reactionToFile = new ReactionToFile
            {
                File = photo,
                Reaction = reaction,
                User = user
            };

            reactionsRepositoy.AddReactionToFile(reactionToFile);
            return true;
        }

        public string GetLocalizedReactionType(ReactionType reactionType)
        {
            switch(reactionType)
            {
                case ReactionType.Like:
                    return "Понравилось";
                case ReactionType.Dislike:
                    return "Не понравилось";
                case ReactionType.Angry:
                    return "Злой смайлик";
                case ReactionType.Cry:
                    return "Грустный смайлик";
                case ReactionType.Heart:
                    return "Сердце";
                case ReactionType.Laugh:
                    return "Смеющийся смайлик";
                case ReactionType.Surprised:
                    return "Удивленный смайлик";
                default:
                    return "Нет";
            }
        }
    }
}