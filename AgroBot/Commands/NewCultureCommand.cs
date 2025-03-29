using AgroBot.Bot;
using AgroBot.Keyboards;
using AgroBot.Models;
using AgroBot.Pipelines.Abstractions;
using AgroBot.Services.Abstractions;
using Telegram.Bot.Types;

namespace AgroBot.Commands
{
    public class NewCultureCommand : ICommand
    {
        private readonly IUserService _userService;
        private readonly IPipelineContextService _pipelineContextService;
        private readonly PipelineHandler _pipeline;
        private readonly BotMessageSender _messageSender;
        private readonly ICropService _cropService;
        private readonly KeyboardMarkupBuilder _keyboardMarkup;
        private readonly ICultureService _cultureService;

        public NewCultureCommand(IUserService userService, IPipelineContextService pipelineContextService, ICropService cropService, PipelineHandler pipelineHandler, BotMessageSender messageSender, KeyboardMarkupBuilder keyboardMarkupBuilder, ICultureService cultureService)
        {
            _pipelineContextService = pipelineContextService;
            _cropService = cropService;
            _userService = userService;
            _pipeline = pipelineHandler;
            _messageSender = messageSender;
            _keyboardMarkup = keyboardMarkupBuilder;
            _cultureService = cultureService;
        }

        public bool CanHandle(string command) => command.Equals("/new_culture", StringComparison.OrdinalIgnoreCase);

        public async Task HandleAsync(Update update)
        {
            if (update.Message == null) return;

            var chatId = update.Message.Chat.Id.ToString();
            var username = update.Message.Chat.Username ?? "Unknown";

            var user = await _userService.GetByChatIdAsync(chatId);
            var isAdded = await _userService.DoesUserExist(user);

            if (isAdded)
            {
                var context = new PipelineContext()
                {
                    Id = Guid.NewGuid().ToString(),
                    ChatId = chatId,
                    Type = PipelineType.CultureCreation,
                    CurrentStep = PipelineStepType.NameStep,
                    Content = string.Empty,
                    IsCompleted = false
                };
                var culture = new Culture
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = string.Empty,
                    Description = string.Empty,
                    AddedTime = DateTime.UtcNow,
                    AuthorId = user.Id,
                    Status = CultureStatus.Draft
                };

                await _pipelineContextService.AddAsync(context);
                await _cultureService.AddAsync(culture);

                await _pipeline.HandlePipelineAsync(context);
            }
            else
            {
                await _messageSender.SendTextMessageAsync(chatId, "You are not registered. Please, use /start command to register.");
            }
        }
    }
}
