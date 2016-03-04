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
        private Post _openPost;


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
                { "O", new Choice("(O)pen post", OpenPost)},
                { "S", new Choice("Clo(S)e post", ClosePost)},
                { "A", new Choice("(A)dd comment", AddComment)},
                { "Q", new Choice("(Q)uit", Quit)}
            };
        }

        private string AddComment()
        {
            var post = _indexer.GetPost(_openPost.Id);
            post.Comments = post.Comments ?? new List<Comment>();
            Console.Write("User: ");
            var user = Console.ReadLine();
            Console.Write("Content: ");
            var content = Console.ReadLine();
            post.Comments.Add(new Comment
            {
                Id = Guid.NewGuid(),
                User = new User { Name = user, UserId = Guid.NewGuid()},
                Content = content,
                TimeStamp = DateTime.UtcNow
            });
            return _indexer.Index(post);
        }

        private string ClosePost()
        {
            _openPost = null;
            return "Post closed";
        }

        private string OpenPost()
        {
            Console.Write("Post index: ");
            int index = int.Parse(Console.ReadLine());
            _openPost = _indexer.GetPost(_postIdCache[index]);
            return "Post opened";
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
                builder.AppendLine(string.Format("{0,-2} - {2,-10} - {1,-40} ({3:dd.MM.yy HH:mm:ss})", _postIdCache.Count, post.Content, post.User.Name, post.TimeStamp.ToLocalTime()));
                foreach (var comment in post.Comments ?? Enumerable.Empty<Comment>())
                {
                    builder.AppendLine(string.Format("     {0,-10} - {1,-40} ({2:dd.MM.yy HH:mm:ss})", comment.User.Name, comment.Content, comment.TimeStamp.ToLocalTime()));
                }
                _postIdCache.Add(post.Id);
            }

            return builder.ToString();
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
                TimeStamp = DateTime.UtcNow,
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
                    if(_openPost != null)
                        Console.WriteLine("Selected post: {0} - {1}", _openPost.Content, _openPost.User.Name);
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
