using PawPaw.Core.Models;

namespace PawPaw.Core
{
    public interface IUserProvider
    {
        User GetCurrentUser();
    }
}