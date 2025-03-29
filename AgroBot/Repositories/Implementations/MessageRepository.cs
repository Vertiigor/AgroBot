using AgroBot.Data;
using AgroBot.Models;
using AgroBot.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AgroBot.Repositories.Implementations
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Message>> GetAllByChatIdAsync(string chatId)
        {
            return await _context.Messages.Where(m => m.ChatId == chatId).ToListAsync();
        }
    }
}
