using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers.Attributes;
using WebApplication1.Enums;
using WebApplication1.Models;
using WebApplication1.Models.DTO.UserRights;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRightsController : ControllerBase
    {
        private MyContext _context = new MyContext();

        [HttpGet("{id}")]
        [SecuredRight(UserRightType.Admin_Read)]
        public IActionResult GetRightsByUserID(int id)
        {
            if (!_context.Users.Any(User => User.UserID == id))
            {
                return StatusCode(404, "User doesn't exist");
            }

            return Ok(_context.UserRights.Where(Right => Right.UserID == id));
        }

        [HttpGet]
        [SecuredRight(UserRightType.Admin_Read)]
        public IActionResult GetRights()
        {
            return Ok(_context.UserRights.ToArray());
        }

        [HttpGet("{id}:{right}")]
        [SecuredRight(UserRightType.Admin_Read)]
        public IActionResult GetRight(int id, UserRightType right)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_context.Users.Any(User => User.UserID == id))
            {
                return StatusCode(404, "User doesn't exist");
            }

            UserRight dbRight = _context.UserRights.Find(id, right.ToString());

            if (dbRight == null)
            {
                return StatusCode(404, "Right doesn't exist");
            }

            return Ok(dbRight);
        }

        [HttpPost("{id}")]
        [SecuredRight(UserRightType.Admin_Add)]
        public IActionResult AddRight(int id, RightDTO right)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_context.Users.Any(User => User.UserID == id))
            {
                return StatusCode(404, "User doesn't exist");
            }
            if (_context.UserRights.Any(Right => Right.UserID == id && Right.Right == right.Right.ToString()))
            {
                return StatusCode(403, "Right already exists");
            }

            UserRight userRight = new UserRight() { UserID = id, Right = right.Right.ToString() };
            _context.Add(userRight);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetRight), new { id = userRight.UserID, right = userRight.Right }, userRight);
        }

        [HttpDelete("{id}:{right}")]
        [SecuredRight(UserRightType.Admin_Edit)]
        public IActionResult DeleteRight(int id, UserRightType right)
        {
            if (!_context.Users.Any(User => User.UserID == id))
            {
                return StatusCode(404, "User doesn't exist");
            }

            UserRight dbRight = _context.UserRights.Find(id, right.ToString());

            if (dbRight == null)
            {
                return StatusCode(404, "Right doesn't exist");
            }

            _context.UserRights.Remove(dbRight);
            _context.SaveChanges();

            return NoContent();
        }
    }
}