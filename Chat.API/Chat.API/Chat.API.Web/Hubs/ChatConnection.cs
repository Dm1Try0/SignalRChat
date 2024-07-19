namespace Chat.API.Web.Hubs
{
    public class ChatConnection
    {
        public DateTime ConnectionTime { get; set; }
        public string ConnectionId { get; set; } = null!;
    }
}
