using System.Text.Json.Serialization;

namespace Chat.WPF.TokenHelper
{
    /// <summary>
    /// Json error
    /// </summary>
    public class SecurityError
    {
        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("error_description")]
        public string Description { get; set; }
    }
}
