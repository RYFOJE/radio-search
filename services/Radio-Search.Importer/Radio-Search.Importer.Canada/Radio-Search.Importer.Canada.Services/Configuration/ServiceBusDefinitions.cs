namespace Radio_Search.Importer.Canada.Services.Configuration
{
    public class ServiceBusDefinitions
    {
        public string TopicName { get; set; } = string.Empty;
        public string ImportStart_SubscriptionName { get; set; } = string.Empty;
        public string DownloadComplete_SubscriptionName { get; set; } = string.Empty;
        public string ChunkReady_SubscriptionName { get; set; } = string.Empty;
        public string ChunkProcessingComplete_SubscriptionName { get; set; } = string.Empty;
    }
}
