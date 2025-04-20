using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers.Attributes;
using WebApplication1.Models;
using WebApplication1.Models.DTO.Users;
using WebApplication1.Enums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private MyContext _context = new MyContext();

        [HttpGet("/Admin")]
        [SecuredRight(UserRightType.Admin_Read)]
        public IActionResult GetUsersAdmin()
        {
            return Ok(_context.Users.ToArray());
        }

        [HttpGet("{id}/Admin")]
        [SecuredRight(UserRightType.Admin_Read)]
        public IActionResult GetUserByIDAdmin(int id)
        {
            User user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserByID(int id)
        {
            User user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            GetUserPublicInfoDTO info = GetUserPublicInfoDTO.FromUser(user);

            return Ok(info);
        }

        [HttpPost]
        public IActionResult CreateUser(CreateUserDTO newUserInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_context.Users.Any(User => User.Username == newUserInfo.Username))
            {
                return StatusCode(403, "Username is already in use");
            }
            if (_context.Users.Any(User => User.Email == newUserInfo.Email))
            {
                return StatusCode(403, "Email already has an asociated account");
            }

            User user = newUserInfo.CreateUser();

            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserByID), new { id = user.UserID }, user);
        }

        [HttpPut("{id}/change-info")]
        public IActionResult UpdateInfo(int id, ChangeInfoDTO info)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Street = info.Street;
            user.City = info.City;
            user.Country = info.Country;
            user.PhoneNumber = info.PhoneNumber;
            user.LastName = info.LastName;
            user.FirstName = info.FirstName;
            user.PostCode = info.PostCode;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}/change-info/Admin")]
        public IActionResult UpdateInfoAdmin(int id, ChangeInfoAdminDTO info)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }
            if (_context.Users.Any(User => User.Username == info.Username && User.UserID != id))
            {
                return StatusCode(403, "Username is already in use");
            }
            if (_context.Users.Any(User => User.Email == info.Email && User.UserID != id))
            {
                return StatusCode(403, "Email already has an asociated account");
            }


            user.Username = info.Username;
            user.Password = info.Password;
            user.Email = info.Email;
            user.CreatedAt = info.CreatedAt;
            user.FirstName = info.FirstName;
            user.LastName = info.LastName;
            user.PhoneNumber = info.PhoneNumber;
            user.City = info.City;
            user.Street = info.Street;
            user.Country = info.Country;
            user.PostCode = info.PostCode;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}/change-unique")]
        public IActionResult UpdateUniqueInfo(int id, ChangeUniqueInfoDTO info)
        {
            User dbUser = _context.Users.Find(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (dbUser == null)
            {
                return NotFound();
            }
            if (_context.Users.Any(User => User.Username == info.Username && User.UserID != id))
            {
                return StatusCode(403, "Username is already in use");
            }
            if (_context.Users.Any(User => User.Email == info.Email && User.UserID != id))
            {
                return StatusCode(403, "Email already has an asociated account");
            }
            if (!BCrypt.Net.BCrypt.EnhancedVerify(info.Password, dbUser.Password))
            {
                return StatusCode(403, "Wrong password");
            }

            dbUser.Email = info.Email;
            dbUser.Username = info.Username;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}/change-password")]
        public IActionResult ChangePassword(int id, ChangePasswordDTO passwords)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            if (!BCrypt.Net.BCrypt.EnhancedVerify(passwords.OldPassword, user.Password))
            {
                return StatusCode(403, "Wrong Password");
            }

            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(passwords.NewPassword);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}/change-password/Admin")]
        public IActionResult ChangePasswordAdmin(int id, PasswordOnlyDTO password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password.Password);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}/Admin")]
        public IActionResult DeleteUserAdmin(int id)
        {
            User user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id, PasswordOnlyDTO password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }
            if (!BCrypt.Net.BCrypt.EnhancedVerify(password.Password, user.Password))
            {
                return StatusCode(403, "Wrong Password");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }
    }
}