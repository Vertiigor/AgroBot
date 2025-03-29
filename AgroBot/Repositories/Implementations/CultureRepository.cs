using AgroBot.Data;
using AgroBot.Models;
using AgroBot.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AgroBot.Repositories.Implementations
{
    public class CultureRepository : Repository<Culture>, ICultureRepository
    {
        public CultureRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Culture>> GetAllByAuthorIdAsync(string authorId)
        {
            return await _context.Cultures.Where(c => c.AuthorId == authorId).ToListAsync();
        }

        public async Task<Culture> GetByAuthorIdAsync(string authorId)
        {
            return await _context.Cultures.FirstOrDefaultAsync(c => c.AuthorId == authorId);
        }
    }
}
