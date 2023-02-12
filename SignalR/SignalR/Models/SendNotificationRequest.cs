namespace SignalR.Models
{
    public class SendNotificationRequest
    {
        public int Id { get; set; }

        public string TitleTemplate { get; set; }

        public string ContentTemplate { get; set; }
    }
}
