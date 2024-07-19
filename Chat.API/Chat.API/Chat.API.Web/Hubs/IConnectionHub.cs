namespace Chat.API.Web.Hubs
{
    public interface IConnectionHub
    {
        /// <summary>
        /// Update users (list)
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        Task UpdateUsersAsync(IEnumerable<string> users);
        /// <summary>
        /// Send message 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessageAsync(string username, string message);
    }
}
