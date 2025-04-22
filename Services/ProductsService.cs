using WebApplication1.Models;
using WebApplication1.Models.DTO.Products;

namespace WebApplication1.Services
{
    public class ProductsService
    {
        

        private MyContext context = new MyContext();
        private RatingsService ratingsService = new RatingsService();
        public ProductPreviewDTO GetProductPreviewData(Product product)
        {
            return new ProductPreviewDTO() 
            {
                ID = product.ProductID,
                Name = product.Name,
                Discount = product.Discount,
                Price = product.Price,
                Rating = ratingsService.GetProductRating(product.ProductID)
            };
        }

        public IQueryable<ProductPreviewDTO> FindProductPreviews()
        {
            var query = this.context.Products.Select(x => new ProductPreviewDTO
            {
                ID = x.ProductID,
                Name = x.Name,
                Discount = x.Discount,
                Price = x.Price,
                Rating = ratingsService.GetProductRating(x.ProductID)
            });
            return query;       
        }
        public bool IsProductValid(Product p)
        {
            if (p.Price <= 0) return false;
            if (p.Discount * p.Price >= p.Price) return false;
            if (this.context.Products.Find(p.ProductID) != null) return false;
            if (this.context.Categories.Find(p.CategoryId) == null) return false;   

            return true;
        } 

    }
}
