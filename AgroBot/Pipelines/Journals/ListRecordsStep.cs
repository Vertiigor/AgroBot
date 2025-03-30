using AgroBot.Bot;
using AgroBot.Models;
using AgroBot.Pipelines.Abstractions;
using AgroBot.Services.Abstractions;
using AgroBot.Services.Implementations;

namespace AgroBot.Pipelines.Journals
{
    public class ListRecordsStep : PipelineStep
    {
        private readonly IJournalService _journalService;
        private readonly ICropService _cropService;

        public ListRecordsStep(BotMessageSender messageSender, IPipelineContextService pipelineContextService, IUserService userService, IJournalService journalService, ICropService cropService) : base(messageSender, pipelineContextService, userService)
        {
            _journalService = journalService;
            _cropService = cropService;
        }

        public async override Task ExecuteAsync(PipelineContext context)
        {

                await _messageSender.SendTextMessageAsync(context.ChatId, "🌱 Here's the journal of this crop.\nBelow is a detailed record of this crop's growth and observations. You can review past entries to monitor progress and make informed decisions.");

                var user = await _userService.GetByChatIdAsync(context.ChatId);
                var crop = await _cropService.GetLastDraftByAuthorIdAsync(user.Id);
                var journals = await _journalService.GetAllByCropIdAsync(crop.Id);

            if (journals.ToList().Count == 0)
            {
                await _messageSender.SendTextMessageAsync(context.ChatId, "⚠️ No records found.\nIt looks like there are no entries for this crop yet. Try adding your first observation to start tracking its growth and development.");
            }
            else
            {
                foreach (var journal in journals)
                {
                    await _messageSender.SendTextMessageAsync(context.ChatId, $"⏱️ Date: {journal.Date}\n📝 Description: {journal.ObservationText}\n🌿 Culture: {crop.Culture}\n📈 Height: {journal.Height}");
                }
            }

            crop.Status = CropStatus.Active;
            await _cropService.UpdateAsync(crop);

            context.StartedDate = DateTime.UtcNow;
            context.CurrentStep = PipelineStepType.Culture;    // Move to the next step
            context.Content = string.Empty;
            context.IsCompleted = true;
            context.FinishedDate = DateTime.UtcNow;
            await _pipelineContextService.DeleteAsync(context.Id);
        }

        public override bool IsApplicable(PipelineContext context)
        {
            return context.CurrentStep == PipelineStepType.ListRecords;
        }
    }
}
