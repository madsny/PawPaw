using System;
using System.Collections.Generic;
using PawPaw.Core.Models;

namespace PawPaw.ElasticSearch
{
    public class PostReader
    {
        private readonly Indexer _indexer;

        public PostReader()
        {
            _indexer = new Indexer();
        }

        public IEnumerable<Post> Search()
        {
            return _indexer.Search();
        }

        public Post GetPost(Guid id)
        {
            return _indexer.GetPost(id);
        }
    }
}
