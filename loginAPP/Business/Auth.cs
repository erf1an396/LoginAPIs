using Microsoft.AspNetCore.Mvc;
using loginAPP.Model;
using loginAPP.ViewModel;
using loginAPP.Context;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Linq.Expressions;

namespace loginAPP.Business
{
    public interface IAuth
    {
        Task<IActionResult> RegisterAsync(RegisterVM model);
        Task<IActionResult> LoginAsync(LoginVM model);

        


    }
    public class Auth : IAuth
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;


        public Auth(AppDbContext db, IConfiguration configuration , UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<IActionResult> RegisterAsync(RegisterVM model)
        {
            try
            {
                var existingUser = await _userManager.FindByNameAsync(model.Username);
                if (existingUser != null)
                    return new BadRequestObjectResult("Username already exist");
                

                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                   
                };

                var result = await _userManager.CreateAsync(user, model.Password);
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

        public async Task<IActionResult> LoginAsync(LoginVM model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return new UnauthorizedObjectResult("invalid username or password");
            }


            var token = GenerateJwtToken(user);
            return new OkObjectResult(new {Token = token});
        }


        


        //private string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
        //private bool VerifyPassword(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);



        private string GenerateJwtToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["jwtConfig:SignInKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name , user.UserName),
                    new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()),
                    new Claim("Test" , user.Id.ToString()),
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
