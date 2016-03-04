using System;
using System.Collections.Generic;
using PawPaw.Core;
using PawPaw.Core.Models;

namespace PawPaw.Cmd
{
    public class CmdUserProvider : IUserProvider
    {
        private readonly Dictionary<string, User> _users;
        private User _currentUser;

        public CmdUserProvider()
        {
            _users = new Dictionary<string, User>();
        }

        public User GetCurrentUser()
        {
            return _currentUser;
        }

        public void SetCurrentUser(string user)
        {
            if(!_users.ContainsKey(user.ToLower()))
            {
                _users[user.ToLower()] = new User {Name = user, UserId = Guid.NewGuid()};
            }
            _currentUser = _users[user.ToLower()];
        }
    }
}
