using AgroBot.Models;

namespace AgroBot.Services.Abstractions
{
    public interface IPipelineContextService : IService<PipelineContext>
    {
        public Task<PipelineContext> GetByChatIdAsync(string chatId);
    }
}
