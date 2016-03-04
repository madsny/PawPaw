using System;
using System.Collections.Generic;
using Nest;
using PawPaw.Core.Models;

namespace PawPaw.ElasticSearch
{
    internal class Indexer
    {
        private readonly ElasticClient _client;
        public const string DefaultIndex = "pawpaw";

        public Indexer()
        {
            var node = new Uri("http://localhost:9200");

            var settings = new ConnectionSettings(
                node,
                defaultIndex: DefaultIndex
            );

            _client = new ElasticClient(settings);
        }

        public void EnsureIndexExists()
        {
            if (!_client.IndexExists(DefaultIndex).Exists)
            {
                EnsureCorrectResponse(_client.CreateIndex(DefaultIndex, c => c.AddMapping<Post>(m => m.MapFromAttributes())));
            }
        }

        public void Index(Post post)
        {
            EnsureCorrectResponse(_client.Index(post, de => de.Id(post.Id.ToString())));
        }

        public void DeleteIndex()
        {
            EnsureCorrectResponse(_client.DeleteIndex(DefaultIndex));
        }

        private void EnsureCorrectResponse(IResponse response)
        {
            if (response.IsValid)
                return;
            throw new Exception(response.ServerError?.Error ?? response.ConnectionStatus.HttpStatusCode?.ToString());
        }

        public IEnumerable<Post> Search()
        {
            var result = _client.Search<Post>(descriptor => descriptor.Query(q => q.MatchAll()));

            return result.Documents;
        }

        public Post GetPost(Guid guid)
        {
            return _client.Get<Post>(guid.ToString()).Source;
        }
    }
}
