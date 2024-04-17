namespace SignalR.SelfHosted.Notifications.Services
{
    public class HubEventName
    {
        private HubEventName(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static HubEventName Create(string value)
        {
            return new HubEventName(value);
        }
    }
}
