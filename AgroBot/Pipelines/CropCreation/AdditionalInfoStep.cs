using AgroBot.Bot;
using AgroBot.Models;
using AgroBot.Pipelines.Abstractions;
using AgroBot.Services.Abstractions;

namespace AgroBot.Pipelines.CropCreation
{
    public class AdditionalInfoStep : PipelineStep
    {
        private readonly ICropService _cropService;

        public AdditionalInfoStep(BotMessageSender messageSender, IPipelineContextService pipelineContextService, IUserService userService, ICropService cropService) : base(messageSender, pipelineContextService, userService)
        {
            _cropService = cropService;
        }

        public async override Task ExecuteAsync(PipelineContext context)
        {
            if (string.IsNullOrEmpty(context.Content))
            {
                await _messageSender.SendTextMessageAsync(context.ChatId, "📝 Write additional info.\nIf there’s anything else important to note about this crop, write it here. You can include special care instructions, potential issues, or future plans for cultivation.");
            }
            else
            {
                var user = await _userService.GetByChatIdAsync(context.ChatId);
                var crop = await _cropService.GetLastDraftByAuthorIdAsync(user.Id);
                crop.AdditionalInfo = context.Content;
                crop.Status = CropStatus.Active;

                await _cropService.UpdateAsync(crop);

                context.StartedDate = DateTime.UtcNow;
                context.CurrentStep = PipelineStepType.SowingDate;    // Move to the next step
                context.Content = string.Empty;

                context.IsCompleted = true;
                context.FinishedDate = DateTime.UtcNow;
                await _pipelineContextService.DeleteAsync(context.Id);

                await _messageSender.SendTextMessageAsync(context.ChatId, "Crop has been created.");
            }
        }

        public override bool IsApplicable(PipelineContext context)
        {
            return context.CurrentStep == PipelineStepType.AdditionalInfo;
        }
    }
}
