using AgroBot.Models;
using AgroBot.Repositories.Abstractions;
using AgroBot.Services.Abstractions;

namespace AgroBot.Services.Implementations
{
    public class CultureService : Service<Culture>, ICultureService
    {
        private readonly ICultureRepository _cultureRepository;

        public CultureService(ICultureRepository repository) : base(repository)
        {
            _cultureRepository = repository;
        }

        public async Task<IEnumerable<Culture>> GetAllByAuthorIdAsync(string authorId)
        {
            return await _cultureRepository.GetAllByAuthorIdAsync(authorId);
        }

        public async Task<Culture> GetByAuthorIdAsync(string authorId)
        {
            return await _cultureRepository.GetByAuthorIdAsync(authorId);
        }

        public async Task<Culture> GetLastActiveByAuthorIdAsync(string authorId)
        {
            var cultures = await GetAllByAuthorIdAsync(authorId);
            var sortedCultures = cultures.Where(p => p.Status == CultureStatus.Active).OrderByDescending(p => p.AddedTime).ToList();

            if (sortedCultures.Count == 0)
                return null;

            return sortedCultures.First();
        }

        public async Task<Culture> GetLastDraftByAuthorIdAsync(string authorId)
        {
            var cultures = await GetAllByAuthorIdAsync(authorId);
            var sortedCultures = cultures.Where(p => p.Status == CultureStatus.Draft).OrderByDescending(p => p.AddedTime).ToList();

            if (sortedCultures.Count == 0)
                return null;

            return sortedCultures.First();
        }
    }
}
