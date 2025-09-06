namespace Radio_Search.Utils.MessageBroker.Implementations
{
    public class MessageFilter
    {

        /// <summary>
        /// The name of the filter
        /// </summary>
        public string FilterName { get; set; } = string.Empty;
        
        /// <summary>
        /// The key on which the filter will filter on
        /// </summary>
        public string FilterOnName { get; set; } = string.Empty;

    }
}
