using AgroBot.Models;
using AgroBot.Pipelines.Abstractions;
using AgroBot.Pipelines.CropCreation;
using AgroBot.Pipelines.CultureCreation;
using AgroBot.Pipelines.Registration;

namespace AgroBot.Bot
{
    public class PipelineHandler
    {
        private readonly Dictionary<PipelineType, Func<PipelineContext, Task>> _handlers;
        private readonly RegistrationPipeline _registrationPipeline;
        private readonly CropCreationPipeline _cropCreationPipeline;
        private readonly CultureCreationPipeline _cultureCreationPipeline;

        public PipelineHandler(IServiceProvider serviceProvider, RegistrationPipeline registrationPipeline, CropCreationPipeline cropCreationPipeline, CultureCreationPipeline cultureCreationPipeline)
        {
            _handlers = new Dictionary<PipelineType, Func<PipelineContext, Task>>
            {
                [PipelineType.Registration] = async (context) => await HandleRegistration(context),
                [PipelineType.CropCreation] = async (context) => await HandleCropCreation(context),
                [PipelineType.CultureCreation] = async (context) => await HandleCultureCreation(context)
            };
            _registrationPipeline = registrationPipeline;
            _cropCreationPipeline = cropCreationPipeline;
            _cultureCreationPipeline = cultureCreationPipeline;
        }

        private async Task HandleCultureCreation(PipelineContext context)
        {
            await _cultureCreationPipeline.ExecuteAsync(context);
        }

        private async Task HandleCropCreation(PipelineContext context)
        {
            await _cropCreationPipeline.ExecuteAsync(context);
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
