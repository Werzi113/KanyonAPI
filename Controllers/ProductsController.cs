using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.dbModels;
using WebApplication1.Models.DTO.Products;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public MyContext Context = new MyContext();
        public ProductsService ProductsService = new ProductsService();
        



        [HttpGet("Previews/Amount")]
        public ObjectResult FindProductPreviews(int amount = -1)
        {
            var items = ProductsService.FindProductPreviews();
            return amount > 0 ? Ok(items.Take(amount).ToArray()) : Ok(items.ToArray());                    
        }
        [HttpGet("Previews/Filter")]
        public ObjectResult FilterProductPreviews(int minPrice, int maxPrice, int minRating, int amount = -1, string name = "")
        {
            var items = ProductsService.FindProductPreviews()
                .Where(preview => preview.Price >= minPrice && preview.Price <= maxPrice && preview.Rating >= minRating);

            items = string.IsNullOrWhiteSpace(name) ? items : items.Where(preview => preview.Name.Contains(name.Trim()));

            return amount > 0 ? Ok(items.Take(amount).ToArray()) : Ok(items.ToArray());
        }

        [HttpGet]
        public ObjectResult FindProducts(int amount = -1)
        {
            return amount > 0 ? Ok(this.Context.Products.Take(amount).ToArray()) : Ok(this.Context.Products.ToArray());
        }

        [HttpGet("{id}")]
        public IActionResult FindProductById(int id)
        {
            Product p = this.Context.Products.Find(id);
            if (p == null)
            {
                return NotFound(new { message = "Product not found " });
            }
            return Ok(p);
        }
        

        [HttpPost("Create")]
        public IActionResult CreateProduct(Product p)
        {
            if (!this.ProductsService.IsProductValid(p))
            {
                return BadRequest(new { message = "Invalid product data" });
            }

            this.Context.Products.Add(p);
            this.Context.SaveChanges();

            return Ok(p);
        }

        [HttpPut("Update:{id}")]
        public IActionResult UpdateProduct(int id, Product newProduct)
        {
            if (!this.ProductsService.IsProductValid(newProduct))
            {
                return BadRequest(new { message = "Invalid product data" });
            }

            Product p = this.Context.Products.Find(id);



            p.Name = newProduct.Name;
            p.Price = newProduct.Price;
            p.Description = newProduct.Description;
            p.CategoryId = newProduct.CategoryId;
            p.Discount = newProduct.Discount;
            p.State = newProduct.State;

            this.Context.SaveChanges();

            return Ok(p);
        }
        [HttpDelete("Delete:{id}")]
        public bool DeleteProduct(int id)
        {
            Product p = this.Context.Products.Find(id);

            this.Context.Products.Remove(p);
            this.Context.SaveChanges();

            return true;
        }



    }
}
