namespace SignalR.Common.Constants
{
    /// <summary>
    /// Represents an static class to hold constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Self-hosted SignalR hub endpoint.
        /// </summary>
        public const string HubEndpoint = "/signalR/notificationHub";

        /// <summary>
        /// Event name for Azure SignalR notification trigger.
        /// </summary>
        public const string NotificationCreatedEvent = "NotificationCreated";

        /// <summary>
        /// Azure SignalR connection key.
        /// </summary>
        public const string AzureSignalRConnectionKey = "AzureSignalR";

        /// <summary>
        /// Name of the Azure SignalR notification Hub.
        /// </summary>
        public const string NotificationHubName = "NotificationHub";
    }
}
