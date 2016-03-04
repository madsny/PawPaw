using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using PawPaw.ElasticSearch.Models;

namespace PawPaw.ElasticSearch
{
    public class Indexer
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

        public string EnsureIndexExists()
        {
            if (!_client.IndexExists(DefaultIndex).Exists)
            {
                return GetResponse(_client.CreateIndex(DefaultIndex, c => c.AddMapping<Post>(m => m.MapFromAttributes())));
            }
            return "Index allready exists";
        }

        public string Index(Post post)
        {
            return GetResponse(_client.Index(post, de => de.Id(post.Id.ToString())));
        }

        public string DeleteIndex()
        {
            return GetResponse(_client.DeleteIndex(DefaultIndex));
        }

        private string GetResponse(IResponse response)
        {
            if (response.IsValid)
                return string.Format("JUPP: {0}", response.ConnectionStatus.HttpStatusCode);
            throw new Exception(response.ServerError?.Error ?? response.ConnectionStatus.HttpStatusCode?.ToString());
        }

        public IEnumerable<Post> Search()
        {
            var result = _client.Search<Post>(descriptor => descriptor.Query(q => q.MatchAll()));

            return result.Documents;
        }
    }
}
