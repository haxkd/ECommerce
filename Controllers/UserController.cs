using EComm.Interface;
using EComm.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web.Http.Cors;

namespace EComm.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : Controller
    {
        public IConfiguration _configuration;        
        private readonly IUsers _IUsers;
        readonly DatabaseContext _context = new();

        public UserController(IUsers IUsers, DatabaseContext dbContext, IConfiguration config)
        {
            _IUsers = IUsers;
            _context = dbContext;
            _configuration = config;
        }

        [HttpPost("signup/")]
        public async Task<ActionResult<User>> SignUp(User user)
        {
            try
            {

            _IUsers.UserSignup(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
            return await Task.FromResult(user);
        }
        //[HttpPost("login/")]
        //public async Task<ActionResult<User>> Login(string email, string password)
        //{
        //    User? x = _IUsers.UserLogin(email, password);
        //    return await Task.FromResult(x);
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Post(UserLogin userLogin)
        {
            if (userLogin.email != null && userLogin.password != null)
            {
                var user = await GetUser(userLogin.email, userLogin.password);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.uid.ToString()),
                        new Claim("Name", user.uname),
                        new Claim("Email", user.uemail)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost("adminlogin")]
        public async Task<IActionResult> AdminPost(AdminLogin adminLogin)
        {
            if (adminLogin.email != null && adminLogin.password != null)
            {
                var admin = await GetAdmin(adminLogin.email, adminLogin.password);

                if (admin != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("AdminId", admin.uid.ToString()),
                        new Claim("Name", admin.uname),
                        new Claim("Email", admin.uemail)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<User> GetUser(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.uemail == email && u.upassword == password && u.isAdmin != "true");
        }

        private async Task<User> GetAdmin(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.uemail == email && u.upassword == password && u.isAdmin=="true");
        }
    }
}
