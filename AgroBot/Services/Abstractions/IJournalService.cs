using AgroBot.Models;

namespace AgroBot.Services.Abstractions
{
    public interface IJournalService : IService<Journal>
    {
        public Task<IEnumerable<Journal>> GetAllByCropIdAsync(string cropId);
        public Task<Journal> GetLastDraftByCropIdAsync(string cropId);
        public Task<Journal> GetLastActiveByCropIdAsync(string cropId);
    }
}
