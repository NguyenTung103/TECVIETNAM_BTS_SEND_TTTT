using Core.Model.ReponseModel;
using Core.Setting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Api_Watec.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public JWT _jwtSetting { get; set; }
        public JwtAccountConfig _jwtAccountConfig { get; set; }
        private static Dictionary<string, string> userRefreshTokens = new Dictionary<string, string>(); // Lưu Refresh Token
        private readonly IConfiguration _config;
        public AuthController(IOptions<JWT> options, IOptions<JwtAccountConfig> optionsAccount, IConfiguration config)
        {
            _jwtSetting = options.Value;
            _jwtAccountConfig = optionsAccount.Value;
            _config = config;
        }
        [HttpPost("login")]
        public ActionResult<AuthModel> Login([FromBody] LoginModel model)
        {
            if (!(model.Username == _jwtAccountConfig.Username && model.Password == _jwtAccountConfig.Password))
            {
                return Unauthorized();
            }            
            var refreshToken = GenerateRefreshToken();

            RefreshTokenStore.Tokens.Add(new UserRefreshToken
            {
                Username = model.Username,
                RefreshToken = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            });
            var accessToken = GenerateJwtToken(model.Username);            

            //userRefreshTokens[model.Username] = refreshToken; // Lưu Refresh Token cho user
            AuthModel result = new AuthModel();
            result.status = 200;
            result.result = new TokenModel { ApiKey = accessToken, RefreshToken = refreshToken };
            return result;
        }
        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] UserRefreshTokenRequest model)
        {
            var storedToken = RefreshTokenStore.Tokens.FirstOrDefault(t => t.RefreshToken == model.RefreshToken);
            if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired refresh token");
            }

            var newAccessToken = GenerateJwtToken(storedToken.Username);
            var newRefreshToken = GenerateRefreshToken();

            // Cập nhật Refresh Token mới
            storedToken.RefreshToken = newRefreshToken;
            storedToken.ExpiryDate = DateTime.UtcNow.AddDays(7);

            return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
        }
        private string GenerateJwtToken(string username)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin")
            }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["AccessTokenExpireMinutes"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class TokenModel
    {
        public string ApiKey { get; set; }
        public string RefreshToken { get; set; }        
    }
    public class AuthModel
    {
        public int status { get; set; }
        public TokenModel result { get; set; }
    }
    public class UserRefreshToken
    {
        public string Username { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
    public class UserRefreshTokenRequest
    {
        public string Username { get; set; }
        public string RefreshToken { get; set; }        
    }
    public static class RefreshTokenStore
    {
        public static List<UserRefreshToken> Tokens = new List<UserRefreshToken>();
    }
}
