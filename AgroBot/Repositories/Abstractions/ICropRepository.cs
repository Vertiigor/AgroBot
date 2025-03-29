using AgroBot.Models;

namespace AgroBot.Repositories.Abstractions
{
    public interface ICropRepository : IRepository<Crop>
    {
        public Task<IEnumerable<Crop>> GetAllByChatId(string chatId);
    }
}
