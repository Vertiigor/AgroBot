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

        public async Task<IEnumerable<Crop>> GetAllByChatId(string chatId)
        {
            return await _cropRepository.GetAllByChatId(chatId);
        }
    }
}
