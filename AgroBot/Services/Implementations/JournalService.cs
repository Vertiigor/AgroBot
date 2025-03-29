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
    }
}
