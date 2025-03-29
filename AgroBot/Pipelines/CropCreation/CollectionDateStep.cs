using AgroBot.Bot;
using AgroBot.Models;
using AgroBot.Pipelines.Abstractions;
using AgroBot.Services.Abstractions;

namespace AgroBot.Pipelines.CropCreation
{
    public class CollectionDateStep : PipelineStep
    {
        public CollectionDateStep(BotMessageSender messageSender, IPipelineContextService pipelineContextService, IUserService userService) : base(messageSender, pipelineContextService, userService)
        {
        }

        public override Task ExecuteAsync(PipelineContext context)
        {
            throw new NotImplementedException();
        }

        public override bool IsApplicable(PipelineContext context)
        {
            return context.CurrentStep == PipelineStepType.CollectingDate;
        }
    }
}
