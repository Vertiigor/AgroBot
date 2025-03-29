using AgroBot.Models;

namespace AgroBot.Services.Abstractions
{
    public interface ICropService : IService<Crop>
    {
        public Task<IEnumerable<Crop>> GetAllByChatId(string chatId);
    }
}
