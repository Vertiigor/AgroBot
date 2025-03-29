using AgroBot.Data;
using AgroBot.Models;
using AgroBot.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AgroBot.Repositories.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context) : base(context) { }

        public async Task<User> GetByChatIdAsync(string chatId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.ChatId == chatId);
        }
    }
}
