using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PawPaw.Core;
using PawPaw.Core.Models;
using PawPaw.ElasticSearch;

namespace PawPaw.Cmd
{
    public class ChoiceMaker
    {
        private readonly AdminService _adminService;
        private readonly PostReader _postReader;
        private readonly PostWritingService _postWritingService;
        private readonly CmdUserProvider _userProvider;
        private readonly Dictionary<string, Choice> _choices;
        private readonly List<Guid> _postIdCache;
        private Post _openPost;


        public ChoiceMaker(AdminService adminService, PostReader postReader, PostWritingService postWritingService, CmdUserProvider userProvider)
        {
            _adminService = adminService;
            _postReader = postReader;
            _postWritingService = postWritingService;
            _userProvider = userProvider;
            _postIdCache = new List<Guid>();
            _choices = new Dictionary<string, Choice>
            {
                { "P", new Choice("(P)rint choices", () => PrintChoices(_choices))},
                { "U", new Choice("Set (u)ser", SetUser)},
                { "D", new Choice("(D)elete index", _adminService.DeleteIndex)},
                { "C", new Choice("(C)reate index", _adminService.EnsureIndexExists) },
                { "I", new Choice("(I)nsert post", InsertPost)},
                { "L", new Choice("(L)ist posts", ListPosts)},
                { "S", new Choice("(S)et current post", SetCurrentPost)},
                { "A", new Choice("(A)dd comment", AddComment)},
                { "Q", new Choice("(Q)uit", () => {})}
            };
        }

        private void SetUser()
        {
            Console.Write("Username: ");
            var user = Console.ReadLine();
            _userProvider.SetCurrentUser(user);
            Console.Clear();
        }

        private void AddComment()
        {
            Console.Write("Content: ");
            var content = Console.ReadLine();
            _postWritingService.CreateComment(_openPost.Id, content);
        }

        private void SetCurrentPost()
        {
            ListPosts();
            Console.Write("Post index ('c' for clear): ");
            var choice = Console.ReadLine();
            if ("c".Equals(choice, StringComparison.InvariantCultureIgnoreCase))
            {
                _openPost = null;
                return;
            }
            int index = int.Parse(choice);
            _openPost = _postReader.GetPost(_postIdCache[index]);
            Console.Clear();
            Console.WriteLine("{0,-10} - {1,-40} ({2:dd.MM.yy HH:mm:ss})", _openPost.User.Name, _openPost.Content, _openPost.Timestamp.ToLocalTime());
            foreach (var comment in _openPost.Comments ?? Enumerable.Empty<Comment>())
            {
                Console.WriteLine("     {0,-10} - {1,-40} ({2:dd.MM.yy HH:mm:ss})", comment.User.Name, comment.Content, comment.Timestamp.ToLocalTime());
            }
        }

        private void ListPosts()
        {
            var posts = _postReader.Search();
            _postIdCache.Clear();
            foreach (var post in posts)
            {
                Console.WriteLine("{0,-2} - {2,-10} - {1,-40} ({3:dd.MM.yy HH:mm:ss})", _postIdCache.Count, post.Content, post.User.Name, post.Timestamp.ToLocalTime());
                foreach (var comment in post.Comments ?? Enumerable.Empty<Comment>())
                {
                    Console.WriteLine("     {0,-10} - {1,-40} ({2:dd.MM.yy HH:mm:ss})", comment.User.Name, comment.Content, comment.Timestamp.ToLocalTime());
                }
                _postIdCache.Add(post.Id);
            }
        }

        private void InsertPost()
        {
            Console.WriteLine("New Post");
            Console.Write("Content: ");
            var content = Console.ReadLine();
            _postWritingService.CreatePost(content);
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
                    if (_userProvider.GetCurrentUser() != null)
                        Console.WriteLine("Current user: {0}", _userProvider.GetCurrentUser().Name);
                    input = Console.ReadLine().Trim().ToUpper();
                    if (_choices.ContainsKey(input))
                    {
                        _choices[input].Action();
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
        
        private static void PrintChoices(Dictionary<string, Choice> choices)
        {
            Console.Clear();
            foreach (var choice in choices.Values)
            {
                Console.WriteLine(choice.Description);
            }
        }
    }
}
