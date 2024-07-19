using Calabonga.AspNetCore.AppDefinitions;
using Chat.AuthService.Infrastructure;
using Chat.AuthService.Web.Definitions.Authorizations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

namespace Chat.AuthService.Web.Definitions.DbContext
{
    /// <summary>
    /// ASP.NET Core services registration and configurations
    /// </summary>
    public class DbContextDefinition : AppDefinition
    {
        /// <summary>
        /// Configure services for current application
        /// </summary>
        /// <param name="builder"></param>
        public override void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(config =>
            {
                // UseInMemoryDatabase - This for demo purposes only!
                //config.UseInMemoryDatabase("DEMO-PURPOSES-ONLY2");
                // Use Npgsql or NpgsqlServer
                config.UseNpgsql("Host=localhost;Port=5432;Database=AuthV1;Username=postgres;Password=789789");
                // config.UseNpgsql(builder.Configuration.GetConnectionString(ApplicationDbContext));

                // Note: use the generic overload if you need to replace the default OpenIddict entities.
                config.UseOpenIddict<Guid>();
            });


            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;
                options.ClaimsIdentity.EmailClaimType = OpenIddictConstants.Claims.Email;
                // configure more options if you need
            });

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireUppercase = false;
                })
                .AddSignInManager()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<ApplicationRoleStore>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>()
                .AddDefaultTokenProviders();

            builder.Services.AddTransient<ApplicationUserStore>();
        }
    }
}
