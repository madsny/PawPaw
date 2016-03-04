using System;
using PawPaw.Core.Commands;

namespace PawPaw.Core
{
    public class PostWritingService
    {
        private readonly IUserProvider _userProvider;
        private readonly PostWritingEngine _postWritingEngine;

        public PostWritingService(IUserProvider userProvider, PostWritingEngine postWritingEngine)
        {
            _userProvider = userProvider;
            _postWritingEngine = postWritingEngine;
        }

        public Guid CreatePost(string content)
        {
            var command = new AddPostCommand
            {
                Id = Guid.NewGuid(),
                Content = content,
                User = _userProvider.GetCurrentUser(),
                Timestamp = DateTime.UtcNow
            };
            _postWritingEngine.Enqueue(command);
            return command.Id;
        }

        public Guid CreateComment(Guid postId, string content)
        {
            var command = new AddCommentCommand
            {
                Id = Guid.NewGuid(),
                Content = content,
                PostId = postId,
                User = _userProvider.GetCurrentUser(),
                Timestamp = DateTime.UtcNow
            };
            _postWritingEngine.Enqueue(command);
            return command.Id;
        }
    }
}
