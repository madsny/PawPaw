using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PawPaw.ElasticSearch;
using PawPaw.ElasticSearch.Models;

namespace PawPaw.Cmd
{
    public class ChoiceMaker
    {
        private readonly Indexer _indexer;
        private readonly Dictionary<string, Choice> _choices;
        private readonly List<Guid> _postIdCache;


        public ChoiceMaker(Indexer indexer)
        {
            _indexer = indexer;
            _postIdCache = new List<Guid>();
            _choices = new Dictionary<string, Choice>
            {
                { "P", new Choice("(P)rint choices", () => PrintChoices(_choices))},
                {"D", new Choice("(D)elete index", indexer.DeleteIndex)},
                {"C", new Choice("(C)reate index", indexer.EnsureIndexExists) },
                { "I", new Choice("(I)nsert post", InsertPost)},
                { "L", new Choice("(L)ist posts", ListPosts)},
                { "Q", new Choice("(Q)uit", Quit)}
            };
        }

        private string Quit()
        {
            return "ByeBye";
        }

        private string ListPosts()
        {
            var posts = _indexer.Search();
            _postIdCache.Clear();
            var builder = new StringBuilder();
            foreach (var post in posts)
            {
                builder.AppendLine(string.Format("{0,2} - {1,-15} - {2,-10} ({3})", _postIdCache.Count, post.Content, post.User.Name, post.Id));
                _postIdCache.Add(post.Id);
            }
            
            return builder.ToString()
        }

        private string InsertPost()
        {
            Console.WriteLine("New Post");
            Console.Write("Content: ");
            var content = Console.ReadLine();
            Console.Write("User: ");
            var user = Console.ReadLine();
            return _indexer.Index(new Post
            {
                Id = Guid.NewGuid(),
                User = new User {Name = user, UserId = Guid.NewGuid()},
                Content = content
            });
        }

        public void RunRunRun()
        {
            string input = "";
            PrintChoices(_choices);
            do
            {
                try
                {
                    input = Console.ReadLine().Trim().ToUpper();
                    if (_choices.ContainsKey(input))
                    {
                        Console.WriteLine(_choices[input].Func());
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Legal choices:");
                        PrintChoices(_choices);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e.Message);
                }

            } while (!input.Trim().Equals("q", StringComparison.InvariantCultureIgnoreCase));
        }
        
        private static string PrintChoices(Dictionary<string, Choice> choices)
        {
            foreach (var choice in choices.Values)
            {
                Console.WriteLine(choice.Description);
            }
            return "OK";
        }
    }
}
