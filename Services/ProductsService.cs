using WebApplication1.Models.dbModels;
using WebApplication1.Models.DTO.Products;

namespace WebApplication1.Services
{
    public class ProductsService
    {
        public const int DEFAULT_RATING = 3;

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
            var query = 
                from p in this.context.Products
                join r in this.context.Ratings on p.ProductID equals r.ProductID into rg
                from r in rg.DefaultIfEmpty()
                group r by new { p.ProductID, p.Name, p.Price, p.Discount } into g
                select new ProductPreviewDTO
                {
                    ID = g.Key.ProductID,
                    Name = g.Key.Name,
                    Price = g.Key.Price,
                    Rating = g.Any() ? (int?)g.Average(r => r.Score) ?? 3 : 3,
                    Discount = g.Key.Discount

                };
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
