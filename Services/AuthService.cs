using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using server.Models;
using server.Repository.Interfaces;

namespace server.Services
{
    public class AuthService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public AuthService(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }

        public string GenerateJwtToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(
                _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not set")
            );

            var expireTime = int.Parse(
                _config["Jwt:AccessTokenExpiryInSeconds"]
                    ?? throw new InvalidOperationException(
                        "Jwt:AccessTokenExpiryInSeconds is not set"
                    )
            );

            var expireDate = DateTime.UtcNow.AddSeconds(expireTime);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    ]
                ),
                Expires = expireDate,
                Issuer =
                    _config["Jwt:Issuer"]
                    ?? throw new InvalidOperationException("Jwt:Issuer is not set"),
                Audience =
                    _config["Jwt:Audience"]
                    ?? throw new InvalidOperationException("Jwt:Audience is not set"),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public int GetRefreshTokenExpirationInDays()
        {
            return int.Parse(
                _config["Jwt:RefreshTokenExpiryInDays"]
                    ?? throw new InvalidOperationException(
                        "Jwt:RefreshTokenExpiryInDays is not set"
                    )
            );
        }

        public async Task<User?> ValidateUserCredentials(string username, string password)
        {
            var findUser = await _userRepository.GetUserByUsernameAsync(username);

            if (findUser == null || !VerifyPassword(password, findUser.PasswordHash))
            {
                return null;
            }

            return findUser;
        }

        public string GenerateRefeshToken()
        {
            return Guid.NewGuid().ToString();
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
