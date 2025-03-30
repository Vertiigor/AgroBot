using AgroBot.Bot;
using AgroBot.Models;
using AgroBot.Pipelines.Abstractions;
using AgroBot.Services.Abstractions;
using AgroBot.Services.Implementations;

namespace AgroBot.Pipelines.CropCreation
{
    public class SowingDateStep : PipelineStep
    {
        private readonly ICropService _cropService;

        public SowingDateStep(BotMessageSender messageSender, IPipelineContextService pipelineContextService, IUserService userService, ICropService cropService) : base(messageSender, pipelineContextService, userService)
        {
            _cropService = cropService;
        }

        public async override Task ExecuteAsync(PipelineContext context)
        {
            if (string.IsNullOrEmpty(context.Content))
            {
                await _messageSender.SendTextMessageAsync(context.ChatId, "🌱 Please enter the sowing date in YYYY-MM-DD format.\nWhen was this crop planted? Enter the date in the format YYYY-MM-DD (e.g., 2025-03-29) to keep an accurate timeline of its growth.");
            }
            else
            {
                string dateString = context.Content;
                DateTime date = DateTime.Parse(dateString);

                var user = await _userService.GetByChatIdAsync(context.ChatId);
                var crop = await _cropService.GetLastDraftByAuthorIdAsync(user.Id);
                crop.SowingDate = date.ToUniversalTime();

                await _cropService.UpdateAsync(crop);

                context.StartedDate = DateTime.UtcNow;
                context.CurrentStep = PipelineStepType.CollectingDate;    // Move to the next step
                context.Content = string.Empty;
                await _pipelineContextService.UpdateAsync(context);
            }
        }

        public override bool IsApplicable(PipelineContext context)
        {
            return context.CurrentStep == PipelineStepType.SowingDate;
        }
    }
}
