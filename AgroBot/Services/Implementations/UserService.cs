using AgroBot.Models;
using AgroBot.Repositories.Abstractions;
using AgroBot.Services.Abstractions;

namespace AgroBot.Services.Implementations
{
    public class UserService : Service<User>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) : base(userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> DoesUserExist(User user)
        {
            if (user == null) return false;

            return await _userRepository.GetByChatIdAsync(user.ChatId) != null;
        }

        public async Task<User> GetByChatIdAsync(string chatId)
        {
            var user = await _userRepository.GetByChatIdAsync(chatId);
            if (user == null)
            {
                return null;
            }

            return user;
        }
    }
}
