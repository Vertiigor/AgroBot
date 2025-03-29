using AgroBot.Models;
using Microsoft.Extensions.Hosting;

namespace AgroBot.Repositories.Abstractions
{
    public interface ICropRepository : IRepository<Crop>
    {
        public Task<Crop> GetByAuthorIdAsync(string authorId);
        public Task<IEnumerable<Crop>> GetAllByAuthorIdAsync(string authorId);

    }
}
