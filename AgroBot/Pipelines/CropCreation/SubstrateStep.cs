using AgroBot.Bot;
using AgroBot.Models;
using AgroBot.Pipelines.Abstractions;
using AgroBot.Services.Abstractions;
using AgroBot.Services.Implementations;

namespace AgroBot.Pipelines.CropCreation
{
    public class SubstrateStep : PipelineStep
    {
        private readonly ICropService _cropService;
        public SubstrateStep(BotMessageSender messageSender, IPipelineContextService pipelineContextService, IUserService userService, ICropService cropService) : base(messageSender, pipelineContextService, userService)
        {
            _cropService = cropService;
        }

        public async override Task ExecuteAsync(PipelineContext context)
        {
            if (string.IsNullOrEmpty(context.Content))
            {
                await _messageSender.SendTextMessageAsync(context.ChatId, "🌿 What substrate do you use?\nPlease specify the type of substrate or growing medium (e.g., soil, hydroponics, coco coir) used for this crop. This information helps in understanding the plant’s growing conditions.");
            }
            else
            {
                var user = await _userService.GetByChatIdAsync(context.ChatId);
                var crop = await _cropService.GetLastDraftByAuthorIdAsync(user.Id);
                crop.Substrate = context.Content;

                await _cropService.UpdateAsync(crop);

                context.StartedDate = DateTime.UtcNow;
                context.CurrentStep = PipelineStepType.SowingDate;    // Move to the next step
                context.Content = string.Empty;
                await _pipelineContextService.UpdateAsync(context);
            }
        }

        public override bool IsApplicable(PipelineContext context)
        {
            return context.CurrentStep == PipelineStepType.Substrate;
        }
    }
}
