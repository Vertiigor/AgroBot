using AgroBot.Data;
using AgroBot.Models;
using AgroBot.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AgroBot.Repositories.Implementations
{
    public class JournalRepository : Repository<Journal>, IJournalRepository
    {
        public JournalRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Journal>> GetAllByCropIdAsync(string cropId)
        {
            return await _context.Journals.Where(c => c.CropId == cropId).ToListAsync();
        }
    }
}
