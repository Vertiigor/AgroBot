using AgroBot.Models;

namespace AgroBot.Repositories.Abstractions
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<User> GetByChatIdAsync(string chatId);
    }
}
