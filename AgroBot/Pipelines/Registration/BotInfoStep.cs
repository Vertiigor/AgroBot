using AgroBot.Bot;
using AgroBot.Models;
using AgroBot.Pipelines.Abstractions;
using AgroBot.Services.Abstractions;

namespace AgroBot.Pipelines.Registration
{
    public class BotInfoStep : PipelineStep
    {
        public BotInfoStep(BotMessageSender messageSender, IPipelineContextService pipelineContextService, IUserService userService) : base(messageSender, pipelineContextService, userService)
        {
        }

        public async override Task ExecuteAsync(PipelineContext context)
        {
            var user = await _userService.GetByChatIdAsync(context.ChatId);
            // Mark the pipeline as completed
            context.IsCompleted = true;
            context.FinishedDate = DateTime.UtcNow;
            await _pipelineContextService.DeleteAsync(context.Id);
            await _messageSender.SendTextMessageAsync(context.ChatId, $"Welcome! You have been registered.");
        }

        public override bool IsApplicable(PipelineContext context)
        {
            return context.CurrentStep == PipelineStepType.BotInfo;
        }
    }
}
