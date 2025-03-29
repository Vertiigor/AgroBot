using AgroBot.Models;

namespace AgroBot.Repositories.Abstractions
{
    public interface ICultureRepository : IRepository<Culture>
    {
        public Task<Culture> GetByAuthorIdAsync(string authorId);
        public Task<IEnumerable<Culture>> GetAllByAuthorIdAsync(string authorId);
    }
}
