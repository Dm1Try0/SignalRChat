namespace Chat.API.Web.Hubs
{
    public class ChatUser
    {
        /// <summary>
        /// List users connections
        /// </summary>
        private readonly List<ChatConnection> _chatConnections;
        public ChatUser(string username)
        {
            _chatConnections = new List<ChatConnection>();
            Username = username;

        }
        public string Username { get; set; } = null!;
     //   public IEnumerable<ChatConnection> Connections { get; }
        public IEnumerable<ChatConnection> Connections => _chatConnections;
        /// <summary>
        /// Open new connection for user
        /// </summary>
        /// <param name="connectionId"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void OpenConnection(string connectionId)
        {
            if(connectionId == null)
            {
                throw new ArgumentNullException(nameof(connectionId));
            }
            var connection = new ChatConnection
            {
                ConnectionTime = DateTime.UtcNow,
                ConnectionId = connectionId
            };
            _chatConnections.Add(connection);
        }
        /// <summary>
        /// Close user connection
        /// </summary>
        public void CloseConnection(string connectionId)
        {
            if (connectionId == null)
            {
                throw new ArgumentNullException(nameof(connectionId));
            }           
            var connection = _chatConnections.SingleOrDefault(x =>  x.ConnectionId.Equals(connectionId));
            if (connection == null)
            {
                return;
            }
            _chatConnections.Remove(connection);
        }

    }
}
