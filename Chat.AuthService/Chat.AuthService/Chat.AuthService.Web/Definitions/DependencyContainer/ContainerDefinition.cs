using Calabonga.AspNetCore.AppDefinitions;
using Chat.AuthService.Web.Application.Services;
using Chat.AuthService.Web.Definitions.Authorizations;

namespace Chat.AuthService.Web.Definitions.DependencyContainer
{
    /// <summary>
    /// Dependency container definition
    /// </summary>
    public class ContainerDefinition : AppDefinition
    {
        public override void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IAccountService, AccountService>();
            builder.Services.AddTransient<ApplicationUserClaimsPrincipalFactory>();
        }
    }
}