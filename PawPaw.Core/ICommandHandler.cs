using PawPaw.Core.Commands;

namespace PawPaw.Core
{
    public interface ICommandHandler
    {
        void Handle(AddCommentCommand command);
        void Handle(AddPostCommand command);
        void Commit();
    }
}