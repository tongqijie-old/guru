using Guru.Formatter.Json;

namespace Guru.AspNetCore.Configuration
{
    public class ApiConfiguration
    {
        public ApiConfiguration()
        {
            Prefix = "api";
        }

        [JsonProperty("prefix")]
        public string Prefix { get; set; }
    }
}