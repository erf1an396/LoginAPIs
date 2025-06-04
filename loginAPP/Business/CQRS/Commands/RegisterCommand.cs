using loginAPP.Model;
using loginAPP.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace loginAPP.Business.CQRS.Commands
{
    public class RegisterCommand : IRequest<IActionResult>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, IActionResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public RegisterCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IActionResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingUser = await _userManager.FindByNameAsync(request.Username);
                if (existingUser != null)
                    return new BadRequestObjectResult("Username already exist");

                var user = new ApplicationUser
                {
                    UserName = request.Username,
                    Email = request.Email,
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    return new BadRequestObjectResult(result.Errors);
                }

                await _userManager.AddToRoleAsync(user, "user");

                var token = GenerateJwtToken(user);
                return new OkObjectResult(new { Token = token });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["jwtConfig:SignInKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim("Test", user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["jwtConfig:SignInKey"],
                Audience = _configuration["jwtConfig:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
} 