using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.DTO.Login;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private TokensService _service = new TokensService();
        private MyContext _context = new MyContext();

        [HttpPost]
        public IActionResult LoginByUsername(LoginDTO login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = _context.Users.FirstOrDefault(User => User.Username == login.Username || User.Email == login.Username);

            if (user == null)
            {
                return NotFound("User doesn't exist");
            }
            if (!BCrypt.Net.BCrypt.EnhancedVerify(login.Password, user.Password))
            {
                return StatusCode(403, "Wrong Password");
            }

            string token = _service.Create(user.UserID);

            return Ok(new { token = token, id = user.UserID });
        }
    }
}