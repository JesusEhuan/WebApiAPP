using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiPenu2.Entities;
using WebApiPenu2.Helpers;
using WebApiPenu2.Models.Users;

namespace WebApiPenu2.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<UserModel> GetAll();
        UserModel GetById(int id);
    }

    public class UserService : IUserService
    {
        private List<UserModel> _users = new List<UserModel>
        {
            new UserModel {Id=1,  UserName = "test", Password = "test" }
        };

        private readonly AppSettings _appSettings;
        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var User = _users.SingleOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);
            if (User == null) return null;

            var Token = generateJwtToken(User);

            return new AuthenticateResponse(User, Token);
        }
        public IEnumerable<UserModel> GetAll()
        {
            return _users;
        }
        private string generateJwtToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserName) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
