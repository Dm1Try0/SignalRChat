
using Chat.API.Infrastructure.DatabaseInitialization;
using Calabonga.AspNetCore.AppDefinitions;

namespace Chat.API.Web.Definitions.DataSeeding
{
    /// <summary>
    /// Seeding DbContext for default data for EntityFrameworkCore
    /// </summary>
    public class DataSeedingDefinition : AppDefinition
    {
        /// <summary>
        /// Configure application for current microservice
        /// </summary>
        /// <param name="app"></param>
        public override void ConfigureApplication(WebApplication app)
            => DatabaseInitializer.Seed(app.Services);
    }
}