using System.Text.Json.Serialization;

namespace Chat.Web.TokenModel
{
    public class SecurityError
    {
        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("error_description")]
        public string Description { get; set; }
    }
}
