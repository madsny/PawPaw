using System;
using System.Collections.Generic;
using PawPaw.Core;
using PawPaw.Core.Commands;
using PawPaw.Core.Models;

namespace PawPaw.ElasticSearch
{
    public class CommandHandler : ICommandHandler
    {
        private readonly Indexer _indexer;

        public CommandHandler()
        {
            _indexer = new Indexer();
        }

        public void Handle(AddCommentCommand command)
        {
            var post = _indexer.GetPost(command.PostId);
            post.Comments = post.Comments ?? new List<Comment>();
            post.Comments.Add(new Comment
            {
                Id = Guid.NewGuid(),
                User = command.User,
                Content = command.Content,
                Timestamp = command.Timestamp
            });
            _indexer.Index(post);
        }

        public void Handle(AddPostCommand command)
        {
            _indexer.Index(new Post
            {
                User = command.User,
                Id = command.Id,
                Content = command.Content,
                Timestamp = command.Timestamp
            });
        }

        public void Commit() {}
    }
}
