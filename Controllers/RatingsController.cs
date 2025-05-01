using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers.Attributes;
using WebApplication1.Models;
using WebApplication1.Enums;
using WebApplication1.Models.DTO.Categories;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.DTO.Ratings;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {

        private MyContext _context = new MyContext();

        [HttpGet("{UserID}:{ProductID}")]
        public IActionResult GetRatingByID(int UserID, int ProductID)
        {
            if (!_context.Users.Any(User => User.UserID == UserID))
            {
                return NotFound("User doesn't exist");
            }
            if (!_context.Products.Any(Product => Product.ProductID == ProductID))
            {
                return NotFound("Product doesn't exist");
            }

            Rating rating = _context.Ratings.Find(UserID, ProductID);

            if (rating == null)
            {
                return NotFound("Rating does not exist");
            }

            return Ok(rating);
        }

        [HttpGet("Exists/{UserID}:{ProductID}")]
        public IActionResult CheckIfRatingExists(int UserID, int ProductID)
        {
            if (!_context.Users.Any(User => User.UserID == UserID))
            {
                return NotFound("User doesn't exist");
            }
            if (!_context.Products.Any(Product => Product.ProductID == ProductID))
            {
                return NotFound("Product doesn't exist");
            }

            Rating rating = _context.Ratings.Find(UserID, ProductID);

            if (rating == null)
            {
                return Ok(false);
            }

            return Ok(true);
        }

        [HttpGet]
        public IActionResult GetRatings()
        {
            return Ok(_context.Ratings.ToArray());
        }

        [HttpGet("User/{id}")]
        public IActionResult GetRatingsByUser(int id)
        {
            if (!_context.Users.Any(User => User.UserID == id))
            {
                return NotFound("User doesn't exist");
            }

            return Ok(_context.Ratings.Where(Rating => Rating.UserID == id).ToArray());
        }

        [HttpGet("Product/{id}")]
        public IActionResult GetRatingsByProduct(int id)
        {
            if (!_context.Products.Any(Product => Product.ProductID == id))
            {
                return NotFound("Product doesn't exist");
            }

            return Ok(_context.Ratings.Where(Rating => Rating.ProductID == id));
        }

        [HttpPost("{id}")]
        [SecuredID]
        public IActionResult AddRating(int id, CreateRatingDTO ratingDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_context.Users.Any(User => User.UserID == id))
            {
                return NotFound("User doesn't exist");
            }
            if (!_context.Products.Any(Product => Product.ProductID == ratingDTO.ProductID))
            {
                return NotFound("Product doesn't exist");
            }
            if (_context.Ratings.Any(Rating => Rating.ProductID == ratingDTO.ProductID && Rating.UserID == id))
            {
                return StatusCode(403, "User already rated product");
            }

            Rating dbRating = ratingDTO.CreateRating(id);

            _context.Ratings.Add(dbRating);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetRatingByID), new { UserID = id, ProductID = ratingDTO.ProductID }, dbRating);
        }

        [HttpPost("Admin")]
        [SecuredRight(UserRightType.Admin_Add)]
        public IActionResult AddRatingAdmin(CreateRatingAdminDTO ratingDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_context.Users.Any(User => User.UserID == ratingDTO.UserID))
            {
                return NotFound("User doesn't exist");
            }
            if (!_context.Products.Any(Product => Product.ProductID == ratingDTO.ProductID))
            {
                return NotFound("Product doesn't exist");
            }
            if (_context.Ratings.Any(Rating => Rating.ProductID == ratingDTO.ProductID && Rating.UserID == ratingDTO.UserID))
            {
                return StatusCode(403, "User already rated product");
            }

            Rating dbRating = ratingDTO.CreateRating();

            _context.Ratings.Add(dbRating);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetRatingByID), new { UserID = ratingDTO.UserID, ProductID = ratingDTO.ProductID }, dbRating);
        }

        [HttpPut("{id}:{ProductID}")]
        [SecuredID]
        public IActionResult EditRating(int id, int ProductID, EditRatingDTO ratingDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_context.Users.Any(User => User.UserID == id))
            {
                return NotFound("User doesn't exist");
            }
            if (!_context.Products.Any(Product => Product.ProductID == ProductID))
            {
                return NotFound("Product doesn't exist");
            }

            Rating dbRating = _context.Ratings.Find(id, ProductID);

            if (dbRating == null)
            {
                return NotFound("Rating doesn't exist");
            }

            dbRating.Description = ratingDTO.Description;
            dbRating.Score = ratingDTO.Score;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}:{ProductID}/Admin")]
        [SecuredRight(UserRightType.Admin_Edit)]
        public IActionResult EditRatingAdmin(int id, int ProductID, EditRatingDTO ratingDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_context.Users.Any(User => User.UserID == id))
            {
                return NotFound("User doesn't exist");
            }
            if (!_context.Products.Any(Product => Product.ProductID == ProductID))
            {
                return NotFound("Product doesn't exist");
            }

            Rating dbRating = _context.Ratings.Find(id, ProductID);

            if (dbRating == null)
            {
                return NotFound("Rating doesn't exist");
            }

            dbRating.Description = ratingDTO.Description;
            dbRating.Score = ratingDTO.Score;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}:{ProductID}")]
        [SecuredID]
        public IActionResult DeleteRating(int id, int ProductID)
        {
            if (!_context.Users.Any(User => User.UserID == id))
            {
                return NotFound("User doesn't exist");
            }
            if (!_context.Products.Any(Product => Product.ProductID == ProductID))
            {
                return NotFound("Product doesn't exist");
            }

            Rating dbRating = _context.Ratings.Find(id, ProductID);

            if (dbRating == null)
            {
                return NotFound("Rating doesn't exist");
            }

            _context.Ratings.Remove(dbRating);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}:{ProductID}/Admin")]
        [SecuredRight(UserRightType.Admin_Edit)]
        public IActionResult DeleteRatingAdmin(int id, int ProductID)
        {
            if (!_context.Users.Any(User => User.UserID == id))
            {
                return NotFound("User doesn't exist");
            }
            if (!_context.Products.Any(Product => Product.ProductID == ProductID))
            {
                return NotFound("Product doesn't exist");
            }

            Rating dbRating = _context.Ratings.Find(id, ProductID);

            if (dbRating == null)
            {
                return NotFound("Rating doesn't exist");
            }

            _context.Ratings.Remove(dbRating);
            _context.SaveChanges();

            return NoContent();
        }
    }
}