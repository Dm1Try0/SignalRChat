using Calabonga.AspNetCore.AppDefinitions;
using Chat.API.Web.Definitions.FluentValidating;
using MediatR;

namespace Chat.API.Web.Definitions.Mediator
{
    /// <summary>
    /// Register Mediator as MicroserviceDefinition
    /// </summary>
    public class MediatorDefinition : AppDefinition
    {
        /// <summary>
        /// Configure services for current microservice
        /// </summary>
        /// <param name="builder"></param>
        public override void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
        }
    }
}