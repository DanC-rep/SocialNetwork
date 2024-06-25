using System.Web;
using Logic.Interfaces;
using Logic.Models;
using Logic.ViewModels;

namespace Application.Services
{
    public class CommentsService
    {
        private readonly ICommentsRepository commentsRepository;
        private readonly UserService userService;

        public CommentsService(ICommentsRepository _commentsRepository, UserService _userService)
        {
            commentsRepository = _commentsRepository;
            userService = _userService;
        }

        public void AddCommentToPhoto(string commentText, User user, FileModel photo)
        {
            var comment = new Comment 
            {
                Text = commentText,
                PublishDate = DateTime.Now,
                User = user
            };
            commentsRepository.AddComment(comment);

            var commentPhoto = new CommentPhoto
            {
                File = photo,
                Comment = comment
            };

            commentsRepository.AddCommentToPhoto(commentPhoto);
        }

        public async Task<List<CommentInfo>> GetPhotoComments(int photoId)
        {
            var comments = commentsRepository.GetPhotoComments(photoId);

            var commentsInfo = new List<CommentInfo>();

            foreach (var comment in comments)
            {
                var user = await userService.GetById(comment.UserId);

                var commentInfo = new CommentInfo
                {
                    CommentText = comment.Text,
                    PublishDate = comment.PublishDate,
                    UserId = user.Id,
                    UserName = user.Name,
                    UserSurname = user.Surname
                };

                commentsInfo.Add(commentInfo);
            }

            return commentsInfo;
        }
    }
}