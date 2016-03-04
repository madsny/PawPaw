using System;

namespace PawPaw.Core.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Content { get; set; }
        public User User { get; set; }
    }
}
