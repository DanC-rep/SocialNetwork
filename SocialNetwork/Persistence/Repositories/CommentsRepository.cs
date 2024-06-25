using Logic.Interfaces;
using Logic.Models;

namespace Persistence.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly NetworkDbContext context;

        public CommentsRepository(NetworkDbContext ctx)
        {
            context = ctx;
        }

        public void AddComment(Comment comment)
        {
            context.Comments.Add(comment);
            context.SaveChanges();
        }

        public void AddCommentToPhoto(CommentPhoto commentPhoto)
        {
            context.PhotosComments.Add(commentPhoto);
            context.SaveChanges();
        }

        public IEnumerable<Comment> GetPhotoComments(int photoId)
        {
            var photoComments = context.PhotosComments.Where(p => p.FileId == photoId);
            var comments = new List<Comment>();

            foreach (var photoComment in photoComments)
            {
                var comment = context.Comments.Where(c => c.Id == photoComment.CommentId).Single();
                comments.Add(comment);
            }

            return comments;
        }
    }
}