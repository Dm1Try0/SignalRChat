namespace Chat.API.Domain.Base
{
    /// <summary>
    /// Static data container
    /// </summary>
    public static partial class AppData
    {
        /// <summary>
        /// CORS Policy name
        /// </summary>
        public const string PolicyName = "CorsPolicy";

        /// <summary>
        /// Current service name
        /// </summary>
        public const string ServiceName = "Microservice Chat.Api";

        /// <summary>
        /// Microservice Chat.Api with integrated OpenIddict for OpenID Connect server and Token Validation
        /// </summary>
        public const string ServiceDescription = "Microservice Chat.Api with integrated OpenIddict for OpenID Connect server and Token Validation";

        /// <summary>
        /// "SystemAdministrator"
        /// </summary>
        public const string SystemAdministratorRoleName = "Administrator";

        /// <summary>
        /// "BusinessOwner"
        /// </summary>
        public const string ManagerRoleName = "Manager";

        /// <summary>
        /// Roles
        /// </summary>
        public static IEnumerable<string> Roles
        {
            get
            {
                yield return SystemAdministratorRoleName;
                yield return ManagerRoleName;
            }
        }
    }
}