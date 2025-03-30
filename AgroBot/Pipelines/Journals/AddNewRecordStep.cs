using AgroBot.Bot;
using AgroBot.Models;
using AgroBot.Pipelines.Abstractions;
using AgroBot.Services.Abstractions;

namespace AgroBot.Pipelines.Journals
{
    public class AddNewRecordStep : PipelineStep
    {
        private readonly IJournalService _journalService;
        private readonly ICropService _cropService;

        public AddNewRecordStep(BotMessageSender messageSender, IPipelineContextService pipelineContextService, IUserService userService, IJournalService journalService, ICropService cropService) : base(messageSender, pipelineContextService, userService)
        {
            _journalService = journalService;
            _cropService = cropService;
        }

        public async override Task ExecuteAsync(PipelineContext context)
        {
            if (string.IsNullOrEmpty(context.Content))
            {
                await _messageSender.SendTextMessageAsync(context.ChatId, "Enter current height.");
            }
            else
            {
                var user = await _userService.GetByChatIdAsync(context.ChatId);
                var crop = await _cropService.GetLastDraftByAuthorIdAsync(user.Id);

                var newJournal = new Journal()
                {
                    Id = Guid.NewGuid().ToString(),
                    Date = DateTime.UtcNow,
                    Photo = new byte[] { },
                    Height = Convert.ToInt32(context.Content),
                    ObservationText = string.Empty,
                    Status = JournalStatus.Draft,
                    CropId = crop.Id
                };

                await _journalService.AddAsync(newJournal);

                context.CurrentStep = PipelineStepType.HeightStep;
                context.Content = string.Empty;

                await _pipelineContextService.UpdateAsync(context);
            }
        }

        public override bool IsApplicable(PipelineContext context)
        {
            return context.CurrentStep == PipelineStepType.AddRecords;
        }
    }
}
