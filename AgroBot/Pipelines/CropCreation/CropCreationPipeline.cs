using AgroBot.Bot;
using AgroBot.Pipelines.Abstractions;

namespace AgroBot.Pipelines.CropCreation
{
    public class CropCreationPipeline : Pipeline
    {
        public CropCreationPipeline(BotMessageSender messageSender, NameStep nameStep, CultureStep cultureStep, SowingDateStep dateStep, SubstrateStep substrateStep, CollectionDateStep collectionDateStep) : base(messageSender)
        {
            _steps.Add(nameStep);
            _steps.Add(cultureStep);
            _steps.Add(dateStep);
            _steps.Add(substrateStep);
            _steps.Add(collectionDateStep);
        }
    }
}
