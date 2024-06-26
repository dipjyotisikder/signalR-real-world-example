namespace SignalR.Api.Hubs.Services
{
    /// <summary>
    /// Represents the value-object for SignalR hub event name.
    /// </summary>
    public class HubEventName
    {
        private HubEventName(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Hub event name value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Represents create method for SignalR Hub Event Name.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static HubEventName Create(string value)
        {
            return new HubEventName(value);
        }
    }
}
