using AgroBot.Bot;
using AgroBot.Models;
using AgroBot.Pipelines.Abstractions;
using AgroBot.Services.Abstractions;
using AgroBot.Services.Implementations;

namespace AgroBot.Pipelines.Journals.Adding
{
    public class ObservationStep : PipelineStep
    {
        private readonly ICropService _cropService;
        private readonly IJournalService _journalService;

        public ObservationStep(BotMessageSender messageSender, IPipelineContextService pipelineContextService, IUserService userService, ICropService cropService, IJournalService journalService) : base(messageSender, pipelineContextService, userService)
        {
            _cropService = cropService;
            _journalService = journalService;
        }

        public async override Task ExecuteAsync(PipelineContext context)
        {
            if (string.IsNullOrEmpty(context.Content))
            {
                await _messageSender.SendTextMessageAsync(context.ChatId, "📝 Enter observation text.\nDescribe your latest observations about the crop. Mention any noticeable changes, health status, or environmental conditions that might affect its growth.");
            }
            else
            {
                var user = await _userService.GetByChatIdAsync(context.ChatId);
                var crop = await _cropService.GetLastDraftByAuthorIdAsync(user.Id);
                var journal = await _journalService.GetLastDraftByCropIdAsync(crop.Id);

                journal.ObservationText = context.Content;
                journal.Status = JournalStatus.Active;

                await _journalService.UpdateAsync(journal);

                crop.Status = CropStatus.Active;
                await _cropService.UpdateAsync(crop);

                context.StartedDate = DateTime.UtcNow;
                context.CurrentStep = PipelineStepType.Culture;    // Move to the next step
                context.Content = string.Empty;
                context.IsCompleted = true;
                context.FinishedDate = DateTime.UtcNow;
                await _pipelineContextService.DeleteAsync(context.Id);
                await _messageSender.SendTextMessageAsync(context.ChatId, "Journal has been successfully created.");
            }
        }

        public override bool IsApplicable(PipelineContext context)
        {
            return context.CurrentStep == PipelineStepType.HeightStep;
        }
    }
}
