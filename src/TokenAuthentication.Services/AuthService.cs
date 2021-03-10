using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TokenAuthentication.Common;
using TokenAuthentication.Dtos;
using TokenAuthentication.Dtos.Authentication;
using TokenAuthentication.Entity.Authentication;
using TokenAuthentication.Entity.Entity;
using TokenAuthentication.Interfaces;

namespace TokenAuthentication.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;        
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;            
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginModel model)
        {

            var user = await _userManager.FindByEmailAsync(model.UserName);
            if (user == null)
            {
                return new ResponseDto<LoginResponseDto>()
                {
                    Message = "Incorrect username/password.",
                };
            }
            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
            {
                return new ResponseDto<LoginResponseDto>()
                {
                    Message = "Incorrect username/password.",
                };
            }
            string token = await GenerateTokenAsync(user);
            return new ResponseDto<LoginResponseDto>()
            {
                Data = new LoginResponseDto() { Token = token, Expiry = DateTime.Now.AddMinutes(10), UserId = user.Id, Email = user.Email },
                Message = "Token generated successfully.",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        private async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:SecretKey").Value);
            var userRoles = await _userManager.GetRolesAsync(user);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                    new Claim(ClaimTypes.DateOfBirth, user.DOB.ToShortDateString())
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _configuration.GetSection("Jwt:Audience").Value,
                Issuer = _configuration.GetSection("Jwt:Issuer").Value,
                Expires = DateTime.Now.AddMinutes(10)
            };
            foreach (var role in userRoles)
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return token;
        }
        
        public LoggedInUser LoggedInUser()
        {
            string token = _httpContextAccessor.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
            return new LoggedInUser()
            {
                Email = _httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == ClaimTypes.Email)?.Value,
                Name = _httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == ClaimTypes.Name)?.Value,
                UserId = _httpContextAccessor.HttpContext.User.FindFirst(p => p.Type == ClaimTypes.Sid)?.Value,
            };
        }
        
    }
}
