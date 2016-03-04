using System;
using Nest;

namespace PawPaw.ElasticSearch.Models
{
    public class User
    {
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed, Type = FieldType.String)]
        public Guid UserId { get; set; }
        public string Name { get; set; }
    }
}