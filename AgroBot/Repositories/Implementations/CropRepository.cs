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

        public async Task<IEnumerable<Crop>> GetAllByAuthorIdAsync(string authorId)
        {
            return await _context.Crops.Where(c => c.AuthorId == authorId).ToListAsync();
        }

        public async Task<Crop> GetByAuthorIdAsync(string authorId)
        {
            return await _context.Crops.FirstOrDefaultAsync(c => c.AuthorId == authorId);
        }
    }
}
