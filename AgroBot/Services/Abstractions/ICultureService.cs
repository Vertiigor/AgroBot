using AgroBot.Models;

namespace AgroBot.Services.Abstractions
{
    public interface ICultureService : IService<Culture>
    {
        public Task<Culture> GetByAuthorIdAsync(string authorId);
        public Task<IEnumerable<Culture>> GetAllByAuthorIdAsync(string authorId);
        public Task<Culture> GetLastDraftByAuthorIdAsync(string authorId);
        public Task<Culture> GetLastActiveByAuthorIdAsync(string authorId);
    }
}
