using AgroBot.Bot;
using AgroBot.Models;
using AgroBot.Pipelines.Abstractions;
using AgroBot.Services.Abstractions;
using AgroBot.Services.Implementations;

namespace AgroBot.Pipelines.CropCreation
{
    public class CultureStep : PipelineStep
    {
        private readonly ICropService _cropService;

        public CultureStep(BotMessageSender messageSender, IPipelineContextService pipelineContextService, IUserService userService, ICropService cropService) : base(messageSender, pipelineContextService, userService)
        {
            _cropService = cropService;
        }

        public async override Task ExecuteAsync(PipelineContext context)
        {
            if (string.IsNullOrEmpty(context.Content))
            {
                await _messageSender.SendTextMessageAsync(context.ChatId, "Please enter the name of the culture.");
            }
            else
            {
                var user = await _userService.GetByChatIdAsync(context.ChatId);
                var crop = await _cropService.GetLastDraftByAuthorIdAsync(user.Id);
                crop.Culture = context.Content;

                await _cropService.UpdateAsync(crop);

                context.StartedDate = DateTime.UtcNow;
                context.CurrentStep = PipelineStepType.Substrate;    // Move to the next step
                context.Content = string.Empty;
                await _pipelineContextService.UpdateAsync(context);
            }
        }

        public override bool IsApplicable(PipelineContext context)
        {
            return context.CurrentStep == PipelineStepType.Culture;
        }
    }
}
