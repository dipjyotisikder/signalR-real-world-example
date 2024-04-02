namespace SignalR.Common.Models
{
    public class NotificationMessageModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}
