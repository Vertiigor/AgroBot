using AgroBot.Data;
using AgroBot.Models;
using AgroBot.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AgroBot.Repositories.Implementations
{
    public class CropRepository : Repository<Crop>, ICropRepository
    {
        public CropRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Crop>> GetAllByChatId(string chatId)
        {
            return await _context.Crops.Where(m => m.ChatId == chatId).ToListAsync();
        }
    }
}
