using Newtonsoft.Json;

namespace demoseusapp.Models
{
    public class UserInfoResponse
    {
        [JsonProperty("dni")]
        public string Dni { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("repository")]
        public string Repository { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}