﻿using System;
using System.Collections.Generic;

namespace PawPaw.ElasticSearch.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Content { get; set; }
        public User User { get; set; } 
        public List<Comment> Comments { get; set; } 
    }
}
