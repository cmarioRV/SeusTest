using Newtonsoft.Json;

namespace demoseusapp
{
    public class AuthorizeResponse
    {
        [JsonProperty("session_id")]
        public string SessionId { get; set; }
    }
}
