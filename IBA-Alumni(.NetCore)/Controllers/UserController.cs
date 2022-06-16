using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace IBA_Alumni_.NetCore_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistration registration)
        {
            if (_context.Users.Any(u => u.Email == registration.Email))
            {
                return BadRequest("User Already Exists.");
            }
            CreatePasswordHash(registration.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Email = registration.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                VerificationToken = CreateRandomToken()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User successfully created");
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(UserLogin login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);
            if(user == null)
            {
                return BadRequest("User not Found.");
            }
            if (!VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Password is incorrect.");
            }
            
            if (user.VerifiedTime == null)
            {
                return BadRequest("User not Verified.");
            }
            return Ok($"Welcome, {user.Email} !!!");
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(string token)
        { 
            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
            if (user == null)
            {
                return BadRequest("Invalid Token.");
            }
            user.VerifiedTime = DateTime.Now;
            await _context.SaveChangesAsync();
            
            return Ok("User is verified !!!");
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return BadRequest("Invalid Token.");
            }
            user.PasswordResetToken = CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddHours(2);
            await _context.SaveChangesAsync();

            return Ok("You can Reset your password now.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword reset)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == reset.Token);
            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return BadRequest("Invalid Token.");
            }
            CreatePasswordHash(reset.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _context.SaveChangesAsync();

            return Ok("Password reset Successfull");
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
               var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }


    }
}
