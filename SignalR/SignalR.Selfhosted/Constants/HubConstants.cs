namespace SignalR.Api.Constants
{
    /// <summary>
    /// Represents an static class to hold constants.
    /// </summary>
    public static class HubConstants
    {
        /// <summary>
        /// Self-hosted SignalR hub endpoint.
        /// </summary>
        public const string HUB_ENDPOINT = "/signalR/applicationHub";

        /// <summary>
        /// Represents a class to keep event key.
        /// </summary>
        public static class Events
        {
            /// <summary>
            /// Represents a Hub event for users joining status.
            /// </summary>
            public const string USER_IS_JOINED = "UserIsJoined";

            /// <summary>
            /// Represents a Hub event for users onLine status.
            /// </summary>
            public const string USER_IS_ONLINE = "UserIsOnLine";

            /// <summary>
            /// Represents a Hub event for users onLine status.
            /// </summary>
            public const string USER_IS_TYPING = "UserIsTyping";

            /// <summary>
            /// Represents a Hub event for users message creation.
            /// </summary>
            public const string MESSAGE_IS_CREATED = "MessageIsCreated";
        }

    }
}
