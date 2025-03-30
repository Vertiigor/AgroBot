using AgroBot.Bot;
using AgroBot.Models;
using AgroBot.Pipelines.Abstractions;
using AgroBot.Services.Abstractions;
using Telegram.Bot.Types;

namespace AgroBot.Pipelines.CultureCreation
{
    public class NameStep : PipelineStep
    {
        private readonly ICultureService _cultureService;

        public NameStep(BotMessageSender messageSender, IPipelineContextService pipelineContextService, IUserService userService, ICultureService cultureService) : base(messageSender, pipelineContextService, userService)
        {
            _cultureService = cultureService;
        }

        public async override Task ExecuteAsync(PipelineContext context)
        {
            if (string.IsNullOrEmpty(context.Content))
            {
                await _messageSender.SendTextMessageAsync(context.ChatId, "🌱 Please enter the name of the culture (type of the plants).\nSpecify the type of plant you are working with. This helps categorize and organize different crops in the system.");
            }
            else
            {
                var user = await _userService.GetByChatIdAsync(context.ChatId);
                var culture = await _cultureService.GetLastDraftByAuthorIdAsync(user.Id);

                culture.Name = context.Content;

                await _cultureService.UpdateAsync(culture);

                context.StartedDate = DateTime.UtcNow;
                context.CurrentStep = PipelineStepType.Description;    // Move to the next step
                context.Content = string.Empty;
                await _pipelineContextService.UpdateAsync(context);
            }
        }

        public override bool IsApplicable(PipelineContext context)
        {
            return context.CurrentStep == PipelineStepType.NameStep;
        }
    }
}
