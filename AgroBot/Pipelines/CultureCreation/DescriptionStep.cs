using AgroBot.Bot;
using AgroBot.Models;
using AgroBot.Pipelines.Abstractions;
using AgroBot.Services.Abstractions;
using AgroBot.Services.Implementations;

namespace AgroBot.Pipelines.CultureCreation
{
    public class DescriptionStep : PipelineStep
    {
        private readonly ICultureService _cultureService;
        public DescriptionStep(BotMessageSender messageSender, IPipelineContextService pipelineContextService, IUserService userService, ICultureService cultureService) : base(messageSender, pipelineContextService, userService)
        {
            _cultureService = cultureService;
        }

        public async override Task ExecuteAsync(PipelineContext context)
        {
            if (string.IsNullOrEmpty(context.Content))
            {
                await _messageSender.SendTextMessageAsync(context.ChatId, "Please, enter the description of the culture.");
            }
            else
            {
                var user = await _userService.GetByChatIdAsync(context.ChatId);
                var culture = await _cultureService.GetLastDraftByAuthorIdAsync(user.Id);

                culture.Description = context.Content;

                await _cultureService.UpdateAsync(culture);

                context.StartedDate = DateTime.UtcNow;
                context.Content = string.Empty;
                context.IsCompleted = true;
                context.FinishedDate = DateTime.UtcNow;
                await _pipelineContextService.DeleteAsync(context.Id);

                await _messageSender.SendTextMessageAsync(context.ChatId, "Culture has been successfully created.");
            }
        }

        public override bool IsApplicable(PipelineContext context)
        {
            return context.CurrentStep == PipelineStepType.Description;
        }
    }
}
