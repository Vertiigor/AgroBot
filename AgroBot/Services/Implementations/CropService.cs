using AgroBot.Models;
using AgroBot.Repositories.Abstractions;
using AgroBot.Services.Abstractions;

namespace AgroBot.Services.Implementations
{
    public class CropService : Service<Crop>, ICropService
    {
        private readonly ICropRepository _cropRepository;

        public CropService(ICropRepository repository) : base(repository)
        {
            _cropRepository = repository;
        }

        public async Task<IEnumerable<Crop>> GetAllByAuthorIdAsync(string authorId)
        {
            return await _cropRepository.GetAllByAuthorIdAsync(authorId);
        }

        public async Task<Crop> GetByAuthorIdAsync(string authorId)
        {
            throw new NotImplementedException();
        }

        public async Task<Crop> GetLastDraftByAuthorIdAsync(string authorId)
        {
            var corps = await GetAllByAuthorIdAsync(authorId);
            var sortedCrops = corps.Where(p => p.Status == CropStatus.Draft).OrderByDescending(p => p.AddedTime).ToList();

            if (sortedCrops.Count == 0)
                return null;

            return sortedCrops.First();
        }

        public async Task<Crop> GetLastActiveByAuthorIdAsync(string authorId)
        {
            var corps = await GetAllByAuthorIdAsync(authorId);
            var sortedCrops = corps.Where(p => p.Status == CropStatus.Active).OrderByDescending(p => p.AddedTime).ToList();

            if (sortedCrops.Count == 0)
                return null;

            return sortedCrops.First();
        }
    }
}
