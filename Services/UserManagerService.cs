using MyHub.Entities;
using MyHub.Repository;

namespace MyHub.Services
{
    public class UserManagerService<TKey> where TKey : IEquatable<TKey>
    {
        private readonly IUserRepository<User<TKey>, TKey> _userRepository;
        public UserManagerService(IUserRepository<User<TKey>, TKey> userRepository)
        {
            _userRepository = userRepository;
        }
    }
}
