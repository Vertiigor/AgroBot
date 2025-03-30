using AgroBot.Bot;
using AgroBot.Models;
using AgroBot.Pipelines.Abstractions;
using AgroBot.Services.Abstractions;
using AgroBot.Services.Implementations;
using Telegram.Bot.Types.ReplyMarkups;

namespace AgroBot.Pipelines.CropCreation
{
    public class CultureStep : PipelineStep
    {
        private readonly ICultureService _cultureService;
        private readonly Keyboards.KeyboardMarkupBuilder _keyboardMarkup;
        private readonly ICropService _cropService;

        public CultureStep(BotMessageSender messageSender, IPipelineContextService pipelineContextService, IUserService userService, ICultureService cultureService, Keyboards.KeyboardMarkupBuilder keyboardMarkup, ICropService cropService) : base(messageSender, pipelineContextService, userService)
        {
            _cultureService = cultureService;
            _keyboardMarkup = keyboardMarkup;
            _cropService = cropService;
        }

        public async override Task ExecuteAsync(PipelineContext context)
        {
            if (string.IsNullOrEmpty(context.Content))
            {
                var cultures = await _cultureService.GetAllAsync();
                var culturesNames = cultures.Select(c => c.Name).ToList();

                List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();

                foreach (var name in culturesNames)
                {
                    var button = _keyboardMarkup.InitializeInlineKeyboardButton(name, $"{CallbackType.ChooseCulture.ToString()}:{name}");
                    buttons.Add(button);
                }

                if (buttons.Count == 0)
                {
                    await _messageSender.SendTextMessageAsync(context.ChatId, "There are no cultures in the database. Please, add a culture first.");
                    context.StartedDate = DateTime.UtcNow;
                    context.CurrentStep = PipelineStepType.Culture;    // Move to the next step
                    context.Content = string.Empty;
                    context.IsCompleted = true;
                    context.FinishedDate = DateTime.UtcNow;
                    await _pipelineContextService.DeleteAsync(context.Id);
                    return;
                }

                var keyboard = _keyboardMarkup.InitializeInlineKeyboardMarkup(buttons);

                // Ask user for the title
                await _messageSender.SendTextMessageAsync(context.ChatId, "🎓Select the culture: ", replyMarkup: keyboard);
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
