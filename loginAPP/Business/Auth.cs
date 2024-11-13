using Microsoft.AspNetCore.Mvc;
using loginAPP.Model;
using loginAPP.ViewModel;
using loginAPP.Context;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

using System.Security.Claims;

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


        public Auth(AppDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<IActionResult> RegisterAsync(RegisterVM model)
        {
            try
            {
                if (await _db.Users.AnyAsync(u => u.Username == model.Username))
                    return new BadRequestObjectResult("Username already Exist");

                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = HashPassword(model.Password)
                };

                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                var token = GenerateJwtToken(user);
                return new OkObjectResult(new
                {
                    Token = token,
                });

            }
            catch (Exception ex)
            {

                throw;
                return null;
            }

        }

        public async Task<IActionResult> LoginAsync(LoginVM model)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
                return new UnauthorizedObjectResult("invalid username or password");


            var token = GenerateJwtToken(user);
            return new OkObjectResult(new {Token = token});
        }


        private string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
        private bool VerifyPassword(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["jwtConfig:SignInKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name , user.Username),
                    new Claim(ClaimTypes.NameIdentifier , user.Id.ToString())
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
