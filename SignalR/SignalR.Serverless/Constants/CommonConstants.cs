namespace SignalR.Serverless.Constants
{
    internal static class CommonConstants
    {
        /// <summary>
        /// Connection key for Azure SignalR.
        /// </summary>
        internal const string AzureSignalRConnectionKey = "AzureSignalR";

        /// <summary>
        /// Event name for Azure SignalR notification trigger.
        /// </summary>
        internal const string NotificationCreatedEvent = "NotificationCreated";

        /// <summary>
        /// Name of the Azure SignalR notification Hub.
        /// </summary>
        internal const string NotificationHubName = "NotificationHub";
    }
}
