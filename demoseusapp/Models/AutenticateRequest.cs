using Newtonsoft.Json;

namespace demoseusapp
{
    public class AutenticateRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("session_id")]
        public string SessionId { get; set; }

        [JsonIgnore]
        public string DocumentType { get; set; }

        [JsonIgnore]
        public string DocumentNumber { get; set; }
    }
}
