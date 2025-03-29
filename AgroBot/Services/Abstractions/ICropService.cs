using AgroBot.Models;
using Microsoft.Extensions.Hosting;

namespace AgroBot.Services.Abstractions
{
    public interface ICropService : IService<Crop>
    {
        public Task<Crop> GetByAuthorIdAsync(string authorId);
        public Task<IEnumerable<Crop>> GetAllByAuthorIdAsync(string authorId);
        public Task<Crop> GetLastDraftByAuthorIdAsync(string authorId);
        public Task<Crop> GetLastActiveByAuthorIdAsync(string authorId);

    }
}
