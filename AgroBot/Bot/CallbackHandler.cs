using AgroBot.Keyboards;
using AgroBot.Models;
using AgroBot.Services.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AgroBot.Bot
{
    public enum CallbackType
    {
        ChooseUniversity,
        ChooseCulture,
        ChooseCrop,
        EditProfile,
        ChooseOption
    }
    public class CallbackHandler
    {
        private readonly Dictionary<string, Func<CallbackQuery, Task>> _handlers;
        private readonly IPipelineContextService _pipelineContextService;
        private readonly ITelegramBotClient _botClient;
        private readonly KeyboardMarkupBuilder _keyboardMarkup;
        private readonly BotMessageSender _messageSender;
        private readonly ICropService _cropService;

        public CallbackHandler(IServiceProvider serviceProvider, IPipelineContextService pipelineContextService, ITelegramBotClient botClient, KeyboardMarkupBuilder keyboardMarkup, BotMessageSender messageSender, ICropService cropService)
        {
            _handlers = new Dictionary<string, Func<CallbackQuery, Task>>
            {
                [CallbackType.ChooseUniversity.ToString()] = async (query) => await HandleChooseUniversity(query),
                [CallbackType.EditProfile.ToString()] = async (query) => await HandleEditProfile(query),
                [CallbackType.ChooseCulture.ToString()] = async (query) => await HandleChooseCulture(query),
                [CallbackType.ChooseCrop.ToString()] = async (query) => await HandleChooseCrop(query),
                [CallbackType.ChooseOption.ToString()] = async (query) => await HandleJournalOption(query)
            };
            _pipelineContextService = pipelineContextService;
            _botClient = botClient;
            _keyboardMarkup = keyboardMarkup;
            _messageSender = messageSender;
            _cropService = cropService;
        }

        private async Task HandleJournalOption(CallbackQuery query)
        {
            // Logic to process profile editing
            // Extract option (assuming format "edit_profile:Option")
            var parts = query.Data.Split(':');
            if (parts.Length < 2)
            {
                Console.WriteLine("Invalid callback data format.");
                return;
            }

            string option = parts[1]; // Extract option from callback data

            var chatId = query.Message.Chat.Id.ToString();
            var messageId = query.Message.MessageId;

            var context = await _pipelineContextService.GetByChatIdAsync(chatId);

            context.Content = option;

            await _pipelineContextService.UpdateAsync(context);

            // Remove inline buttons after selection
            await _keyboardMarkup.RemoveKeyboardAsync(_botClient, chatId, messageId);

            await _messageSender.EditTestMessageAsync(chatId, messageId, $"You've selected {option} as option.");
        }

        private async Task HandleChooseCrop(CallbackQuery query)
        {
            // Logic to process university selection
            // Extract university name (assuming format "choose_university:University Name")
            var parts = query.Data.Split(':');
            if (parts.Length < 2)
            {
                Console.WriteLine("Invalid callback data format.");
                return;
            }

            string crop = parts[1]; // Extract name from callback data

            var chatId = query.Message.Chat.Id.ToString();
            var messageId = query.Message.Id;
            var context = await _pipelineContextService.GetByChatIdAsync(chatId);

            var crops = await _cropService.GetAllAsync();
            var chosenCrop = crops.FirstOrDefault(c => c.Name == crop);

            chosenCrop.Status = CropStatus.Draft;

            await _cropService.UpdateAsync(chosenCrop);

            context.Content = crop;

            await _pipelineContextService.UpdateAsync(context);

            // Remove inline buttons after selection
            await _keyboardMarkup.RemoveKeyboardAsync(_botClient, chatId, messageId);

            await _messageSender.EditTestMessageAsync(chatId, messageId, $"You've selected {crop} as your crop.");
        }

        private async Task HandleChooseCulture(CallbackQuery query)
        {
            // Logic to process university selection
            // Extract university name (assuming format "choose_university:University Name")
            var parts = query.Data.Split(':');
            if (parts.Length < 2)
            {
                Console.WriteLine("Invalid callback data format.");
                return;
            }

            string cultureName = parts[1]; // Extract name from callback data

            var chatId = query.Message.Chat.Id.ToString();
            var messageId = query.Message.Id;

            var context = await _pipelineContextService.GetByChatIdAsync(chatId);

            context.Content = cultureName;

            await _pipelineContextService.UpdateAsync(context);

            // Remove inline buttons after selection
            await _keyboardMarkup.RemoveKeyboardAsync(_botClient, chatId, messageId);

            await _messageSender.EditTestMessageAsync(chatId, messageId, $"You've selected {cultureName} as your culture.");
        }

        public async Task HandleCallbackAsync(CallbackQuery query)
        {
            if (query == null)
            {
                return;
            }

            var parts = query.Data.Split(':');
            var handlerName = parts[0];

            if (_handlers.TryGetValue(handlerName, out var handler))
            {
                await handler(query);
            }
            else
            {
                Console.WriteLine($"Unknown callback data: {query.Data}");
            }
        }

        private async Task HandleChooseUniversity(CallbackQuery query)
        {
            // Logic to process university selection
            // Extract university name (assuming format "choose_university:University Name")
            var parts = query.Data.Split(':');
            if (parts.Length < 2)
            {
                Console.WriteLine("Invalid callback data format.");
                return;
            }

            string universityName = parts[1]; // Extract name from callback data

            var chatId = query.Message.Chat.Id.ToString();
            var messageId = query.Message.Id;

            var context = await _pipelineContextService.GetByChatIdAsync(chatId);

            context.Content = universityName;

            await _pipelineContextService.UpdateAsync(context);

            // Remove inline buttons after selection
            await _keyboardMarkup.RemoveKeyboardAsync(_botClient, chatId, messageId);

            await _messageSender.EditTestMessageAsync(chatId, messageId, $"You've selected {universityName} as your university.");
        }

        private async Task HandleEditProfile(CallbackQuery query)
        {
            // Logic to process profile editing
            // Extract option (assuming format "edit_profile:Option")
            var parts = query.Data.Split(':');
            if (parts.Length < 2)
            {
                Console.WriteLine("Invalid callback data format.");
                return;
            }

            string option = parts[1]; // Extract option from callback data

            var chatId = query.Message.Chat.Id.ToString();
            var messageId = query.Message.MessageId;

            var context = await _pipelineContextService.GetByChatIdAsync(chatId);

            context.Content = option;

            await _pipelineContextService.UpdateAsync(context);

            // Remove inline buttons after selection
            await _keyboardMarkup.RemoveKeyboardAsync(_botClient, chatId, messageId);

            await _messageSender.EditTestMessageAsync(chatId, messageId, $"You've selected {option} as your profile option.");
        }
    }
}