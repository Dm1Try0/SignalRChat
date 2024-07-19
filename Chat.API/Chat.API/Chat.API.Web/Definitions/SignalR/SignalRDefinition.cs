using Calabonga.AspNetCore.AppDefinitions;
using Chat.API.Web.Hubs;
namespace Chat.API.Web.Definitions.SignalR
{
    /// <summary>
    /// SignalR definition for application
    /// </summary>
    public class SignalRDefinition : AppDefinition
    {
        public override void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<ChatController>();
        }
        public override void ConfigureApplication(WebApplication app)
        {
            app.MapHub<ConnectionHub>("/chat");
        }
    }
}
