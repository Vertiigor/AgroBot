﻿using AgroBot.Bot;
using AgroBot.Models;
using AgroBot.Pipelines.Abstractions;
using AgroBot.Services.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AgroBot.Commands
{
    public class StartCommand : ICommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IUserService _userService;
        private readonly IPipelineContextService _pipelineContextService;
        private readonly PipelineHandler _pipeline;
        private readonly BotMessageSender _messageSender;

        public StartCommand(ITelegramBotClient botClient, IUserService userService, IPipelineContextService pipelineContextService, PipelineHandler pipelineHandler, BotMessageSender messageSender)
        {
            _botClient = botClient;
            _pipelineContextService = pipelineContextService;
            _userService = userService;
            _pipeline = pipelineHandler;
            _messageSender = messageSender;
        }

        public bool CanHandle(string command) => command.Equals("/start", StringComparison.OrdinalIgnoreCase);


        public async Task HandleAsync(Update update)
        {
            if (update.Message == null) return;

            var chatId = update.Message.Chat.Id.ToString();
            var username = update.Message.Chat.Username ?? "Unknown";

            var user = await _userService.GetByChatIdAsync(chatId);

            var isAdded = await _userService.DoesUserExist(user);

            if (isAdded == false)
            {
                await _userService.AddAsync(new Models.User
                {
                    ChatId = chatId,
                    UserName = username,
                    NormalizedUserName = username.ToUpper(),
                    JoinedDate = DateTime.UtcNow,
                });

                var context = new PipelineContext()
                {
                    Id = Guid.NewGuid().ToString(),
                    ChatId = chatId,
                    Type = PipelineType.Registration,
                    CurrentStep = PipelineStepType.BotInfo,
                    Content = string.Empty,
                    IsCompleted = false
                };

                await _pipelineContextService.AddAsync(context);
                await _pipeline.HandlePipelineAsync(context);
            }
            else
            {
                await _messageSender.SendTextMessageAsync(chatId, "You are already register.");
            }
        }
    }
}
