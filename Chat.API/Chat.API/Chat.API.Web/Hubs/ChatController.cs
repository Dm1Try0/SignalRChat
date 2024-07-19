namespace Chat.API.Web.Hubs
{
    public class ChatController
    {
        public List<ChatUser> Users { get; } = new();
        public void ConnectUser(string username,string connectionId)
        {
            var userExists = GetConnectedUserByUsername(username);
            if (userExists != null)
            {
                userExists.OpenConnection(connectionId);
                return;
            }

            var user = new ChatUser(username);
            user.OpenConnection(connectionId);
            Users.Add(user);

        }
        public bool DisconnectUser(string connectionId)
        {
            var userExists = GetConnectedUserById(connectionId);
            if (userExists == null)
            {
                return false;
            }
            if(!userExists.Connections.Any())
            {
                return false; // шанс низок но всё бывает
            }
            var connectionExists = userExists.Connections.Select(x => x.ConnectionId).First().Equals(connectionId);
            if (!connectionExists)
            {
                return false; // так же.
            }
            if(userExists.Connections.Count() == 1)
            {
                Users.Remove(userExists);
                return true;
            }
            userExists.CloseConnection(connectionId);
            return false;
        }
        /// <summary>
        /// Get connected user by connection ID
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        private ChatUser? GetConnectedUserById(string connectionId)
        {
            return Users.FirstOrDefault(x => x.Connections.Select(c => c.ConnectionId)
                        .Contains(connectionId));
        }
        /// <summary>
        ///  Get connected user by Username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private ChatUser? GetConnectedUserByUsername(string username)
        {
            return Users.FirstOrDefault(x => string.Equals(
                        x.Username,
                        username,
                        StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
