using Microsoft.AspNetCore.SignalR;

namespace Chat.API.Web.Hubs
{
    public class ConnectionHub : Hub<IConnectionHub>
    {
        public readonly ChatController _chatController;
        /// <summary>
        /// Default group (can change to list)
        /// </summary>
        private const string _defaultGroup = "Main";
        public ConnectionHub(ChatController chatController) 
        {
            _chatController = chatController;
        }
        /// <summary>
        /// New connection with hub
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var username = Context.User?.Identity?.Name ?? "UnknownUser";
            var connectionId = Context.ConnectionId;
            _chatController.ConnectUser(username, connectionId);
            await Groups.AddToGroupAsync(connectionId, _defaultGroup);
            await UpdateUsersAsync();
            await base.OnConnectedAsync();
        }
        /// <summary>
        /// Disconnection user with hub
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
           var userDisconnected = _chatController.DisconnectUser(Context.ConnectionId);
            if(!userDisconnected)
            {
                await base.OnDisconnectedAsync(exception);
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, _defaultGroup);
            await UpdateUsersAsync();
            await base.OnDisconnectedAsync(exception);
        }
        /// <summary>
        /// Update users async example: for groups
        /// </summary>
        /// <returns></returns>
        public async Task UpdateUsersAsync()
        {
            var users = _chatController.Users.Select(x => x.Username).ToList();
            await Clients.All.UpdateUsersAsync(users);
        }
        /// <summary>
        /// Send message(string)
        /// </summary>
        /// <param name="username"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(string username, string message)
        {
            await Clients.Others.SendMessageAsync(username, message);
        }
    }
}
