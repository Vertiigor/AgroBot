using AgroBot.Bot;
using AgroBot.Keyboards;
using AgroBot.Models;
using AgroBot.Pipelines.Abstractions;
using AgroBot.Services.Abstractions;
using Telegram.Bot.Types.ReplyMarkups;

namespace AgroBot.Pipelines.Journals
{
    public class CropChoosingStep : PipelineStep
    {
        private readonly KeyboardMarkupBuilder _keyboardMarkup;
        private readonly ICropService _cropService;

        public CropChoosingStep(BotMessageSender messageSender, IPipelineContextService pipelineContextService, IUserService userService, KeyboardMarkupBuilder keyboardMarkupBuilder, ICropService cropService) : base(messageSender, pipelineContextService, userService)
        {
            _keyboardMarkup = keyboardMarkupBuilder;
            _cropService = cropService;
        }

        public async override Task ExecuteAsync(PipelineContext context)
        {
            if (string.IsNullOrEmpty(context.Content))
            {
                var user = await _userService.GetByChatIdAsync(context.ChatId);
                var crops = await _cropService.GetAllByAuthorIdAsync(user.Id);

                if (crops.Any())
                {
                    var names = crops.Select(c => c.Name).ToList();
                    List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();

                    foreach (var name in names)
                    {
                        var button = _keyboardMarkup.InitializeInlineKeyboardButton(name, $"{CallbackType.ChooseCrop.ToString()}:{name}");
                        buttons.Add(button);
                    }

                    var keyboard = _keyboardMarkup.InitializeInlineKeyboardMarkup(buttons);

                    // Ask user for the title
                    await _messageSender.SendTextMessageAsync(context.ChatId, "🌾 Select the crop:\nPlease choose the crop you want to manage from the list below. If the crop is not listed, consider adding it to your records.", replyMarkup: keyboard);
                }
                else
                {
                    await _messageSender.SendTextMessageAsync(context.ChatId, "You have no crops. Please, use /new_crop command to add a crop.");
                    context.StartedDate = DateTime.UtcNow;
                    context.CurrentStep = PipelineStepType.Culture;    // Move to the next step
                    context.Content = string.Empty;
                    context.IsCompleted = true;
                    context.FinishedDate = DateTime.UtcNow;
                    await _pipelineContextService.DeleteAsync(context.Id);
                    return;
                }
            }
            else
            {
                //ParseNextStep(context);

                context.CurrentStep = PipelineStepType.ChoosingOption;

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
            return context.CurrentStep == PipelineStepType.ChoosingCrop;
        }
    }
}
