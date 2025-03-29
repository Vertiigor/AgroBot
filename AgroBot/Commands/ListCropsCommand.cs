using AgroBot.Bot;
using AgroBot.Keyboards;
using AgroBot.Services.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AgroBot.Commands
{
    public class ListCropsCommand : ICommand
    {
        private readonly IUserService _userService;
        private readonly IPipelineContextService _pipelineContextService;
        private readonly PipelineHandler _pipeline;
        private readonly BotMessageSender _messageSender;
        private readonly ICropService _cropService;
        private readonly KeyboardMarkupBuilder _keyboardMarkup;

        public ListCropsCommand(IUserService userService, IPipelineContextService pipelineContextService, ICropService cropService, PipelineHandler pipelineHandler, BotMessageSender messageSender, KeyboardMarkupBuilder keyboardMarkupBuilder)
        {
            _pipelineContextService = pipelineContextService;
            _cropService = cropService;
            _userService = userService;
            _pipeline = pipelineHandler;
            _messageSender = messageSender;
            _keyboardMarkup = keyboardMarkupBuilder;
        }

        public bool CanHandle(string command) => command.Equals("/listcrops", StringComparison.OrdinalIgnoreCase);

        public async Task HandleAsync(Update update)
        {
            if (update.Message == null) return;

            var chatId = update.Message.Chat.Id.ToString();
            var username = update.Message.Chat.Username ?? "Unknown";

            var user = await _userService.GetByChatIdAsync(chatId);

            var isAdded = await _userService.DoesUserExist(user);

            if (isAdded == false)
            {
                var crops = await _cropService.GetAllByAuthorIdAsync(user.Id);

                if (crops.Any())
                {
                    var buttons = new List<InlineKeyboardButton>();
                    foreach (var crop in crops)
                    {
                        buttons.Add(_keyboardMarkup.InitializeInlineKeyboardButton(crop.Name, crop.Id));
                    }
                    var keyboard = _keyboardMarkup.InitializeInlineKeyboardMarkup(buttons);
                    await _messageSender.SendTextMessageAsync(chatId, "Here is the list of your crops:", keyboard);
                }
                else
                {
                    await _messageSender.SendTextMessageAsync(chatId, "You have no crops. Please, use /addcrop command to add a crop.");
                }
            }
            else
            {
                await _messageSender.SendTextMessageAsync(chatId, "You are not registered. Please, use /start command to register.");
            }
        }
    }
}
