using Core.Setting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (!(model.Username == _jwtAccountConfig.Username && model.Password == _jwtAccountConfig.Password))
            {
                return Unauthorized();
            }

            var accessToken = GenerateJwtToken(model.Username);
            var refreshToken = GenerateRefreshToken();

            userRefreshTokens[model.Username] = refreshToken; // Lưu Refresh Token cho user

            return Ok(new TokenModel { AccessToken = accessToken, RefreshToken = refreshToken });
        }
        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] TokenModel tokenModel)
        {
            var principal = GetPrincipalFromExpiredToken(tokenModel.AccessToken);
            if (principal == null)
            {
                return Unauthorized("Không tồn tại access token");
            }

            var username = principal.Identity.Name;
            if (!userRefreshTokens.TryGetValue(username, out var savedRefreshToken) || savedRefreshToken != tokenModel.RefreshToken)
            {
                return Unauthorized("Không tồn tại refresh token");
            }

            var newAccessToken = GenerateJwtToken(username);
            var newRefreshToken = GenerateRefreshToken();

            userRefreshTokens[username] = newRefreshToken; // Cập nhật Refresh Token mới

            return Ok(new TokenModel { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
        }
        [HttpPost("revoke")]
        public IActionResult Revoke([FromBody] string username)
        {
            if (userRefreshTokens.ContainsKey(username))
            {
                userRefreshTokens.Remove(username);
                return Ok("Refresh Token revoked.");
            }
            return NotFound("User not found.");
        }
        private string GenerateJwtToken(string username)
        {
            var key = Encoding.UTF8.GetBytes(_jwtSetting.ScretKey);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSetting.Issuer,
                audience: _jwtSetting.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(1), // Token ngắn hạn
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.UTF8.GetBytes(_jwtSetting.ScretKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSetting.Issuer,
                ValidAudience = _jwtSetting.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class TokenModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
