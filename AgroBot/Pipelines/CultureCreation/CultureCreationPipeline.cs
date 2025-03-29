using AgroBot.Bot;
using AgroBot.Pipelines.Abstractions;

namespace AgroBot.Pipelines.CultureCreation
{
    public class CultureCreationPipeline : Pipeline
    {
        public CultureCreationPipeline(BotMessageSender messageSender, NameStep nameStep, DescriptionStep descriptionStep) : base(messageSender)
        {
            _steps.Add(nameStep);
            _steps.Add(descriptionStep);
        }
    }
}
