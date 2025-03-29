using AgroBot.Bot;
using AgroBot.Services.Abstractions;
using Telegram.Bot.Types;

namespace AgroBot.Commands
{
    public class ListCropsCommand : ICommand
    {
        private readonly IUserService _userService;
        private readonly IPipelineContextService _pipelineContextService;
        private readonly PipelineHandler _pipeline;
        private readonly BotMessageSender _messageSender;

        public ListCropsCommand(IUserService userService, IPipelineContextService pipelineContextService, PipelineHandler pipelineHandler, BotMessageSender messageSender)
        {
            _pipelineContextService = pipelineContextService;
            _userService = userService;
            _pipeline = pipelineHandler;
            _messageSender = messageSender;
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

            }
            else
            {
                await _messageSender.SendTextMessageAsync(chatId, "You are not registered. Please, use /start command to register.");
            }
        }
    }
}
