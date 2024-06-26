namespace SignalR.Api.Constants
{
    /// <summary>
    /// Represents an static class to hold constants.
    /// </summary>
    public static class AzureHubConstants
    {
        /// <summary>
        /// Event name for Azure SignalR notification trigger.
        /// </summary>
        public const string NotificationCreatedEvent = "NotificationCreated";

        /// <summary>
        /// Azure SignalR connection key.
        /// </summary>
        public const string AzureSignalRConnectionKey = "AzureSignalR";

        /// <summary>
        /// Name of the Azure SignalR Hub.
        /// </summary>
        public const string HubName = "ApplicationHub";
    }
}
