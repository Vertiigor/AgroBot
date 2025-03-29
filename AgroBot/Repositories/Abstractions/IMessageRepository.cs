using AgroBot.Models;

namespace AgroBot.Repositories.Abstractions
{
    public interface IMessageRepository : IRepository<Message>
    {
        public Task<IEnumerable<Message>> GetAllByChatIdAsync(string chatId);
    }
}
