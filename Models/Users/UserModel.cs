using System.Text.Json.Serialization;

namespace WebApiPenu2.Models.Users
{
    public class UserModel
    {
        public string UserName { get; set; }
        
        [JsonIgnore]
        public string Password { get; set; }
    }
}
