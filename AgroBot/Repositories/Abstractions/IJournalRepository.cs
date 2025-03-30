using AgroBot.Models;

namespace AgroBot.Repositories.Abstractions
{
    public interface IJournalRepository : IRepository<Journal>
    {
        public Task<IEnumerable<Journal>> GetAllByCropIdAsync(string cropId);
    }
}
