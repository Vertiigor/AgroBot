using AgroBot.Models;
using AgroBot.Repositories.Abstractions;
using AgroBot.Services.Abstractions;

namespace AgroBot.Services.Implementations
{
    public class JournalService : Service<Journal>, IJournalService
    {
        private readonly IJournalRepository _journalRepository;
        public JournalService(IJournalRepository repository) : base(repository)
        {
            _journalRepository = repository;
        }

        public async Task<IEnumerable<Journal>> GetAllByCropIdAsync(string cropId)
        {
            return await _journalRepository.GetAllByCropIdAsync(cropId);
        }

        public async Task<Journal> GetLastActiveByCropIdAsync(string cropId)
        {
            var journals = await GetAllByCropIdAsync(cropId);
            var sortedJournals = journals.Where(p => p.Status == JournalStatus.Active).OrderByDescending(p => p.Date).ToList();

            if (sortedJournals.Count == 0)
                return null;

            return sortedJournals.First();
        }

        public async Task<Journal> GetLastDraftByCropIdAsync(string cropId)
        {
            var journals = await GetAllByCropIdAsync(cropId);
            var sortedJournals = journals.Where(p => p.Status == JournalStatus.Draft).OrderByDescending(p => p.Date).ToList();

            if (sortedJournals.Count == 0)
                return null;

            return sortedJournals.First();
        }
    }
}
