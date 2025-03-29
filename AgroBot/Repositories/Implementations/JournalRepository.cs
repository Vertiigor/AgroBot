using AgroBot.Data;
using AgroBot.Models;
using AgroBot.Repositories.Abstractions;

namespace AgroBot.Repositories.Implementations
{
    public class JournalRepository : Repository<Journal>, IJournalRepository
    {
        public JournalRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
