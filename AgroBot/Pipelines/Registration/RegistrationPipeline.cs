using AgroBot.Bot;
using AgroBot.Pipelines.Abstractions;

namespace AgroBot.Pipelines.Registration
{
    public class RegistrationPipeline : Pipeline
    {
        public RegistrationPipeline(BotMessageSender messageSender, BotInfoStep botInfoStep) : base(messageSender)
        {
            _steps.Add(botInfoStep);
        }
    }
}
