using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StatusPageAPI.Dtos.AuthDtos;
using StatusPageAPI.Helpers;
using StatusPageAPI.Models.Configurations;

namespace StatusPageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtHelperService _jwtHelperService;
        private readonly AuthenticationConfig _authConfig;

        public AuthController(IOptions<AuthenticationConfig> authConfig, JwtHelperService jwtHelperService)
        {
            _jwtHelperService = jwtHelperService;
            _authConfig = authConfig.Value;
        }
        
        [HttpPost("login")]
        public ActionResult<LoginReturnDto> Login(UserForLoginDto userForLoginDto)
        {
            
            if (!userForLoginDto.Username.Equals(_authConfig.Username) || !userForLoginDto.Password.Equals(_authConfig.Password))
                return Unauthorized();

            var key = _jwtHelperService.JwtKey;
            
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(),
                Expires = DateTime.UtcNow.AddDays(_authConfig.DaysUntilTokenExpiration),
                NotBefore = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(1)),
                SigningCredentials = creds,
                Issuer = _authConfig.TokenIssuer
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new LoginReturnDto()
            {
                Token = tokenHandler.WriteToken(token),
            });
        }
    }
}