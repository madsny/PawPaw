using System;
using PawPaw.Core.Models;

namespace PawPaw.Core.Commands
{
    public class AddPostCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public User User { get; set; }
        public DateTime Timestamp { get; set; }
        public void Accept(ICommandHandler commandHandler)
        {
            commandHandler.Handle(this);
        }
    }
}
