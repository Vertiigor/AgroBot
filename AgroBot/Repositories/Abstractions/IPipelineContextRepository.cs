using AgroBot.Models;

namespace AgroBot.Repositories.Abstractions
{
    public interface IPipelineContextRepository : IRepository<PipelineContext>
    {
        public Task<PipelineContext> GetByChatIdAsync(string chatId);
    }
}
