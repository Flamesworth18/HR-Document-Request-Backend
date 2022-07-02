using Document_Request.Data;
using Document_Request.Models.Auth;
using Document_Request.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Document_Request.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _config;

        public AccountsController(DatabaseContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        //Create user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register request)
        {
            if (
               string.IsNullOrEmpty(request.FirstName) &&
               string.IsNullOrEmpty(request.LastName) &&
               string.IsNullOrEmpty(request.Email) &&
               string.IsNullOrEmpty(request.Username) &&
               string.IsNullOrEmpty(request.Password) &&
               string.IsNullOrEmpty(request.ConfirmPassword) &&
               string.IsNullOrEmpty(request.Sex))
            {
                return BadRequest("Invalid inputs");
            }

            var userExists = await _context.Users.Where(x => x.Username == request.Username).FirstOrDefaultAsync();
            if (userExists != null)
            {
                return BadRequest("Username already exists!");
            }

            TokenGenerator.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User();
            user.Id = Guid.NewGuid();
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Username = request.Username;
            user.Email = request.Email;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Role = "User";
            user.Sex = request.Sex;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var subUser = new SubUser();
            subUser.Id = user.Id;
            subUser.FirstName = request.FirstName;

            return Ok(subUser);
        }

        //Log in user
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login request)
        {
            if (string.IsNullOrEmpty(request.Username) && string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Invalid inputs");
            }

            var user = await _context.Users.Where(x => x.Username == request.Username).FirstOrDefaultAsync();
            if (user != null)
            {

                if (!TokenGenerator.VerifyPasswordHash(user, request))
                {
                    return BadRequest("Wrong Password");
                }

                var token = TokenGenerator.CreateToken(user, _config);
                return Ok(token);
            }

            return NotFound("User does not exist");
        }


        //Log out user
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] Logout request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
            if(user != null)
            {
                return Ok("User successfully logout!");
            }

            return BadRequest("Email not found!");
        }

        //Forgot password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] Forgot forgot)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == forgot.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            user.PasswordResetToken = TokenGenerator.CreateRandomPasswordToken();
            user.PasswordTokenExpires = DateTime.Now.AddDays(1);
            await _context.SaveChangesAsync();

            return Ok(user.PasswordResetToken);
        }

        //Reset password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(Reset request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (user == null || user.PasswordTokenExpires < DateTime.Now)
            {
                return BadRequest("Invalid Token.");
            }

            TokenGenerator.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.PasswordTokenExpires = null;

            await _context.SaveChangesAsync();

            return Ok("Password successfully reset.");
        }
    }
}
