using AgroBot.Bot;
using AgroBot.Models;
using AgroBot.Services.Abstractions;

namespace AgroBot.Pipelines.Abstractions
{
    public enum PipelineStepType
    {
        BotInfo,

        NameStep,
        Culture,
        SowingDate,
        Substrate,
        CollectingDate,
        AdditionalInfo,

        Description,

        ChoosingOption,
        ChoosingCrop,
        ListRecords,
        AddRecords,

        HeightStep
    }

    public abstract class PipelineStep
    {
        protected readonly BotMessageSender _messageSender;
        protected readonly IPipelineContextService _pipelineContextService;
        protected readonly IUserService _userService;

        protected PipelineStep(BotMessageSender messageSender, IPipelineContextService pipelineContextService, IUserService userService)
        {
            _messageSender = messageSender;
            _pipelineContextService = pipelineContextService;
            _userService = userService;
        }

        public abstract Task ExecuteAsync(PipelineContext context);

        public abstract bool IsApplicable(PipelineContext context);
    }
}
