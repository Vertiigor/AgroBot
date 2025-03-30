using AgroBot.Bot;
using AgroBot.Keyboards;
using AgroBot.Models;
using AgroBot.Pipelines.Abstractions;
using AgroBot.Services.Abstractions;
using AgroBot.Services.Implementations;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace AgroBot.Pipelines.Journals
{
    public class OptionStep : PipelineStep
    {
        private readonly KeyboardMarkupBuilder _keyboardMarkup;
        private readonly ICropService _cropService;

        public OptionStep(BotMessageSender messageSender, IPipelineContextService pipelineContextService, IUserService userService, KeyboardMarkupBuilder keyboardMarkup, ICropService cropService) : base(messageSender, pipelineContextService, userService)
        {
            _keyboardMarkup = keyboardMarkup;
            _cropService = cropService;
        }

        public async override Task ExecuteAsync(PipelineContext context)
        {
            if (string.IsNullOrEmpty(context.Content))
            {
                var options = new List<string> { "Add", "List" };
                List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();

                foreach (var option in options)
                {
                    var button = _keyboardMarkup.InitializeInlineKeyboardButton(option, $"{CallbackType.ChooseOption.ToString()}:{option}Records");
                    buttons.Add(button);
                }

                var keyboard = _keyboardMarkup.InitializeInlineKeyboardMarkup(buttons);

                // Ask user for the title
                await _messageSender.SendTextMessageAsync(context.ChatId, "🎓Do you wanna add or check records of this crop?", replyMarkup: keyboard);
            }
            else
            {
                ParseNextStep(context);

                context.Content = string.Empty;

                await _pipelineContextService.UpdateAsync(context);
            }
        }

        private static void ParseNextStep(PipelineContext context)
        {
            if (System.Enum.TryParse(context.Content, out PipelineStepType stepType))
            {
                context.CurrentStep = stepType;
            }
            else
            {
                Console.WriteLine("Invalid step type.");
                return;
            }
        }

        public override bool IsApplicable(PipelineContext context)
        {
            return context.CurrentStep == PipelineStepType.ChoosingOption;
        }
    }
}
