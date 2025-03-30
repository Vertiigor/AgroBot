using AgroBot.Models;
using AgroBot.Pipelines.Abstractions;
using AgroBot.Pipelines.CropCreation;
using AgroBot.Pipelines.CultureCreation;
using AgroBot.Pipelines.Journals;
using AgroBot.Pipelines.Registration;

namespace AgroBot.Bot
{
    public class PipelineHandler
    {
        private readonly Dictionary<PipelineType, Func<PipelineContext, Task>> _handlers;
        private readonly RegistrationPipeline _registrationPipeline;
        private readonly CropCreationPipeline _cropCreationPipeline;
        private readonly CultureCreationPipeline _cultureCreationPipeline;
        private readonly JournalPipeline _journalPipeline;

        public PipelineHandler(IServiceProvider serviceProvider, RegistrationPipeline registrationPipeline, CropCreationPipeline cropCreationPipeline, CultureCreationPipeline cultureCreationPipeline, JournalPipeline journalPipeline)
        {
            _handlers = new Dictionary<PipelineType, Func<PipelineContext, Task>>
            {
                [PipelineType.Registration] = async (context) => await HandleRegistration(context),
                [PipelineType.CropCreation] = async (context) => await HandleCropCreation(context),
                [PipelineType.CultureCreation] = async (context) => await HandleCultureCreation(context),
                [PipelineType.Journal] = async (context) => await HandleJournal(context)
            };
            _registrationPipeline = registrationPipeline;
            _cropCreationPipeline = cropCreationPipeline;
            _cultureCreationPipeline = cultureCreationPipeline;
            _journalPipeline = journalPipeline;
        }

        private async Task HandleJournal(PipelineContext context)
        {
            await _journalPipeline.ExecuteAsync(context);
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
