using AgroBot.Bot;
using AgroBot.Pipelines.Abstractions;
using AgroBot.Pipelines.Journals.Adding;

namespace AgroBot.Pipelines.Journals
{
    public class JournalPipeline : Pipeline
    {
        public JournalPipeline(BotMessageSender messageSender, OptionStep optionStep, ListRecordsStep listRecordsStep, AddNewRecordStep addNewRecordStep, CropChoosingStep cropChoosingStep, ObservationStep observationStep) : base(messageSender)
        {
            _steps.Add(observationStep);
            _steps.Add(cropChoosingStep);
            _steps.Add(optionStep);
            _steps.Add(listRecordsStep);
            _steps.Add(addNewRecordStep);
        }
    }
}
