using Logic.Models;

namespace Logic.Interfaces
{
    public interface ICommentsRepository
    {
        void AddComment(Comment comment);
        void AddCommentToPhoto(CommentPhoto commentPhoto);
        IEnumerable<Comment> GetPhotoComments(int photoId);
    }
}