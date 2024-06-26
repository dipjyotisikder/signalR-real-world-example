namespace SignalR.Api.Constants
{
    /// <summary>
    /// Represents an static class to hold constants.
    /// </summary>
    public static class AuthenticationConstants
    {
        /// <summary>
        /// Secret Key for token.
        /// </summary>
        public const string TOKEN_SECRET_KEY = "VeryPowerfulElegantFreakingSuperSecretKey";

        /// <summary>
        /// Issuer for token.
        /// </summary>
        public const string ISSUER = "http://localhost:4200/";

        /// <summary>
        /// Audience for token.
        /// </summary>
        public const string AUDIENCE = "http://localhost:5000/";
    }
}
