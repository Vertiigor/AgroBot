using AgroBot.Models;
using AgroBot.Repositories.Abstractions;
using AgroBot.Services.Abstractions;

namespace AgroBot.Services.Implementations
{
    public class PipelineContextService : Service<PipelineContext>, IPipelineContextService
    {
        private readonly IPipelineContextRepository _pipelineRepository;

        public PipelineContextService(IPipelineContextRepository repository) : base(repository)
        {
            _pipelineRepository = repository;
        }

        public async Task<PipelineContext> GetByChatIdAsync(string chatId)
        {
            var pipeline = await _pipelineRepository.GetByChatIdAsync(chatId);
            if (pipeline == null)
            {
                return null;
            }

            return pipeline;
        }
    }
}
