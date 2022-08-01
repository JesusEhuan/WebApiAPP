using WebApiPenu2.Models.Users;

namespace WebApiPenu2.Entities
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(UserModel user, string token)
        {
            UserName = user.UserName;
            Token = token;
        }
    }
}
