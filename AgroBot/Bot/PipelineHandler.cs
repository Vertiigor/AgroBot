using AgroBot.Models;
using AgroBot.Pipelines.Abstractions;
using AgroBot.Pipelines.Registration;

namespace AgroBot.Bot
{
    public class PipelineHandler
    {
        private readonly Dictionary<PipelineType, Func<PipelineContext, Task>> _handlers;
        private readonly RegistrationPipeline _registrationPipeline;

        public PipelineHandler(IServiceProvider serviceProvider, RegistrationPipeline registrationPipeline)
        {
            _handlers = new Dictionary<PipelineType, Func<PipelineContext, Task>>
            {
                [PipelineType.Registration] = async (context) => await HandleRegistration(context)
            };
            _registrationPipeline = registrationPipeline;
        }

        private async Task HandleRegistration(PipelineContext context)
        {
            await _registrationPipeline.ExecuteAsync(context);
        }

        public async Task HandlePipelineAsync(PipelineContext context)
        {
            if (context == null)
            {
                return;
            }
            if (_handlers.TryGetValue(context.Type, out var handler))
            {
                await handler(context);
            }
        }
    }
}
