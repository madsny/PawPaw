using System;

namespace PawPaw.ElasticSearch.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public User User { get; set; } 
    }
}
