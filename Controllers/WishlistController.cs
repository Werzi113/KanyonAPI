using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers.Attributes;
using WebApplication1.Models;
using WebApplication1.Enums;
using WebApplication1.Models.DTO.Wishlist;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private MyContext _context = new MyContext();

        [HttpGet("User/{id}/Previews")]
        [SecuredID]
        public IActionResult GetWishlistProductPreviews(int id)
        {
            if (!_context.Users.Any(User => User.UserID == id))
            {
                return NotFound("User doesn't exist");
            }

            List<WishlistProductDTO> products = new List<WishlistProductDTO>();

            foreach (var item in _context.WishlistedProducts.Where(product => product.UserID == id).ToArray())
            {
                Product product = _context.Products.Find(item.ProductID);

                if (product == null)
                {
                    continue;
                }

                WishlistProductDTO productDTO = new WishlistProductDTO() {
                    Discount = product.Discount,
                    ProductID = product.ProductID,
                    Price = product.Price,
                    Name = product.Name,
                };

                var img = _context.ProductPictures.FirstOrDefault(pic => pic.IsPreview == true && pic.ProductID == product.ProductID);

                if (img == null)
                {
                    productDTO.ImageURL = "";
                }
                else
                {
                    string baseUrl = $"{Request.Scheme}://{Request.Host}";
                    productDTO.ImageURL = $"{baseUrl}{img.PicturePath}";
                }

                products.Add(productDTO);
            }

            return Ok(products.ToArray());
        }

        [HttpGet("User/{id}")]
        [SecuredID]
        public IActionResult GetWishlistByUserID(int id)
        {
            if (!_context.Users.Any(User => User.UserID == id))
            {
                return NotFound("User doesn't exist");
            }

            return Ok(_context.WishlistedProducts.Where(Wish => Wish.UserID == id).ToArray());
        }

        [HttpGet("User/{id}/Admin")]
        [SecuredRight(UserRightType.Admin_Read)]
        public IActionResult GetWishlistByUserIDAdmin(int id)
        {
            if (!_context.Users.Any(User => User.UserID == id))
            {
                return NotFound("User doesn't exist");
            }

            return Ok(_context.WishlistedProducts.Where(Wish => Wish.UserID == id).ToArray());
        }

        [HttpGet("Wishlists")]
        [SecuredRight(UserRightType.Admin_Read)]
        public IActionResult GetWishlists()
        {
            return Ok(_context.WishlistedProducts.ToArray());
        }

        [HttpGet("{id}:{productID}/Admin")]
        [SecuredRight(UserRightType.Admin_Read)]
        public IActionResult GetWishlistedProductAdmin(int id, int productID)
        {
            if (!_context.Users.Any(User => User.UserID == id))
            {
                return NotFound("User doesn't exist");
            }
            if (!_context.Products.Any(Product => Product.ProductID == productID))
            {
                return NotFound("Product doesn't exist");
            }

            WishlistedProduct product = _context.WishlistedProducts.Find(productID, id);

            if (product == null)
            {
                return NotFound("Entry doesn't exist");
            }

            return Ok(product);
        }

        [HttpGet("{id}:{productID}")]
        [SecuredID]
        public IActionResult GetWishlistedProduct(int id, int productID)
        {
            if (!_context.Users.Any(User => User.UserID == id))
            {
                return NotFound("User doesn't exist");
            }
            if (!_context.Products.Any(Product => Product.ProductID == productID))
            {
                return NotFound("Product doesn't exist");
            }

            WishlistedProduct product = _context.WishlistedProducts.Find(productID, id);

            if (product == null)
            {
                return NotFound("Entry doesn't exist");
            }

            return Ok(product);
        }

        [HttpPost("{id}:{productID}")]
        [SecuredID]
        public IActionResult AddToWishlist(int id, int productID)
        {
            if (!_context.Users.Any(User => User.UserID == id))
            {
                return NotFound("User doesn't exist");
            }
            if (!_context.Products.Any(Product => Product.ProductID == productID))
            {
                return NotFound("Product doesn't exist");
            }
            if (_context.WishlistedProducts.Any(Product => Product.ProductID == productID && Product.UserID == id))
            {
                return StatusCode(403, "Product is already wishlisted");
            }

            WishlistedProduct wishlistedProduct = new WishlistedProduct() { ProductID = productID, UserID = id };
            _context.WishlistedProducts.Add(wishlistedProduct);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetWishlistByUserID), new { id = id, productID = productID }, wishlistedProduct);
        }

        [HttpPost("{id}:{productID}/Admin")]
        [SecuredRight(UserRightType.Admin_Add)]
        public IActionResult AddToWishlistAdmin(int id, int productID)
        {
            if (!_context.Users.Any(User => User.UserID == id))
            {
                return NotFound("User doesn't exist");
            }
            if (!_context.Products.Any(Product => Product.ProductID == productID))
            {
                return NotFound("Product doesn't exist");
            }
            if (_context.WishlistedProducts.Any(Product => Product.ProductID == productID && Product.UserID == id))
            {
                return StatusCode(403, "Product is already wishlisted");
            }

            WishlistedProduct wishlistedProduct = new WishlistedProduct() { ProductID = productID, UserID = id };
            _context.WishlistedProducts.Add(wishlistedProduct);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetWishlistByUserIDAdmin), new { id = id, productID = productID }, wishlistedProduct);
        }

        [HttpDelete("{id}:{ProductID}")]
        [SecuredID]
        public IActionResult DeleteWishlistedProduct(int id, int productID)
        {
            if (!_context.Users.Any(User => User.UserID == id))
            {
                return NotFound("User doesn't exist");
            }
            if (!_context.Products.Any(Product => Product.ProductID == productID))
            {
                return NotFound("Product doesn't exist");
            }

            WishlistedProduct product = _context.WishlistedProducts.Find(productID, id);

            if (product == null)
            {
                return NotFound("Product isn't wishlisted");
            }

            _context.WishlistedProducts.Remove(product);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}:{ProductID}/Admin")]
        [SecuredRight(UserRightType.Admin_Edit)]
        public IActionResult DeleteWishlistedProductAdmin(int id, int productID)
        {
            if (!_context.Users.Any(User => User.UserID == id))
            {
                return NotFound("User doesn't exist");
            }
            if (!_context.Products.Any(Product => Product.ProductID == productID))
            {
                return NotFound("Product doesn't exist");
            }

            WishlistedProduct product = _context.WishlistedProducts.Find(productID, id);

            if (product == null)
            {
                return NotFound("Product isn't wishlisted");
            }

            _context.WishlistedProducts.Remove(product);
            _context.SaveChanges();

            return NoContent();
        }
    }
}