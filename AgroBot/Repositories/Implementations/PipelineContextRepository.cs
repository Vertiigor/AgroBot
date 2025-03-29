using AgroBot.Data;
using AgroBot.Models;
using AgroBot.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AgroBot.Repositories.Implementations
{
    public class PipelineContextRepository : Repository<PipelineContext>, IPipelineContextRepository
    {
        public PipelineContextRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<PipelineContext?> GetByChatIdAsync(string chatId)
        {
            return await _context.Pipelines.FirstOrDefaultAsync(p => p.ChatId == chatId);
        }
    }
}
