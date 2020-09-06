using Newtonsoft.Json;

namespace ATF.Contracts
{
    public class AuthObjects
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "instance_url")]
        public string InstanceUrl { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }
    }
}
