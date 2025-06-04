using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers.Attributes;
using WebApplication1.Models;
using WebApplication1.Models;
using WebApplication1.Models.DTO.Products;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private MyContext context = new MyContext();
        private ProductsService productsService = new ProductsService();
        private FilterService filterService = new FilterService();



        [HttpGet("Previews/Amount")]
        public ObjectResult GetProductPreviews(int amount = -1)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            var items = productsService.FindProductPreviews(baseUrl);
            return amount > 0 ? Ok(items.Take(amount).ToArray()) : Ok(items.ToArray());                    
        }
        [HttpGet("Previews/admin")]
        public ObjectResult GetAdminProductPreviewas()
        {
            var list = this.context.Products.Select(p => new AdminProductPreview()
            {
                Discount = p.Discount,
                Name = p.Name,
                Price = p.Price,
                productId = p.ProductID
            }).ToList();
            return Ok(list);
        }
        [HttpPost("Previews/Filter")]
        public ObjectResult FilterProductPreviews(ProductFilter filter, int amount = -1)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            var items = productsService.FindProductPreviews(baseUrl);
            items = filterService.FilterByPriceRange(items, filter.MinPrice, filter.MaxPrice);
            items = filterService.FilterByRating(items, filter.MinRating);
            items = filterService.FilterByName(items, filter.Name);
            items = filterService.FilterByCategory(items, filter.CategoryID);

            return amount > 0 ? Ok(items.Take(amount)) : Ok(items);
        }

        [HttpGet("ProductNamePairs")]
        public IActionResult GetProductNamePairs()
        {
            var res = context.Products.Select(item =>
                new ProductNamePairDTO()
                {
                    Name = item.Name,
                    ProductID = item.ProductID,
                }
            );

            return Ok(res.ToArray());
        }

        [HttpGet]
        public ObjectResult FindProducts(int amount = -1)
        {
            return amount > 0 ? Ok(this.context.Products.Take(amount).ToArray()) : Ok(this.context.Products.ToArray());
        }

        [HttpGet("{id}")]
        public IActionResult FindProductById(int id)
        {
            Product? p = this.context.Products.Find(id);
            if (p == null)
            {
                return NotFound(new { message = "Product not found " });
            }
            return Ok(p);
        }
        

        [HttpPost("Create")]
        public IActionResult CreateProduct(Product p)
        {
            if (!this.productsService.IsProductValid(p))
            {
                return BadRequest(new { message = "Invalid product data" });
            }

            this.context.Products.Add(p);
            this.context.SaveChanges();

            return Ok(p);
        }

        [HttpPut("Update:{productId}")]
        public IActionResult UpdateProduct(int productId, Product newProduct)
        {
            if (!this.productsService.IsProductValid(newProduct))
            {
                return BadRequest(new { message = "Invalid product data" });
            }

            Product p = this.context.Products.Find(productId);



            p.Name = newProduct.Name;
            p.Price = newProduct.Price;
            p.Description = newProduct.Description;
            p.CategoryId = newProduct.CategoryId;
            p.Discount = newProduct.Discount;
            p.State = newProduct.State;

            this.context.SaveChanges();

            return Ok(p);
        }
        [HttpDelete("Delete:{id}")]
        public bool DeleteProduct(int id)
        {
            Product? p = this.context.Products.Find(id);
            if (p == null)
            {
                return false;
            }

            var pictures = this.context.ProductPictures.Where(pic => pic.ProductID == id).ToList();
            this.context.ProductPictures.RemoveRange(pictures);
            this.context.SaveChanges();

            var service = new ProductPicturesService();
            service.DeleteProductPictures(id);


            this.context.Products.Remove(p);
            this.context.SaveChanges();

            return true;
        }



    }
}
