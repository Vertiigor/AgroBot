using Telegram.Bot.Types;

namespace AgroBot.Commands
{
    public interface ICommand
    {
        public Task HandleAsync(Update update);
        public bool CanHandle(string command);
    }
}
