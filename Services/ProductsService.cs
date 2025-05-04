using WebApplication1.Models;
using WebApplication1.Models.DTO.Products;

namespace WebApplication1.Services
{
    public class ProductsService
    {
        

        private MyContext context = new MyContext();
        private RatingsService ratingsService = new RatingsService();
        private ProductPicturesService pictureService = new ProductPicturesService();
        //public ProductPreviewDTO GetProductPreview(Product product)
        //{
        //    return new ProductPreviewDTO() 
        //    {
        //        ID = product.ProductID,
        //        ImgUrl = pictureService.GetProductPreviewPicturePath
        //        Name = product.Name,
        //        Discount = product.Discount,
        //        Price = product.Price,
        //        Rating = ratingsService.GetProductRating(product.ProductID)
        //    };
        //}

        public IQueryable<ProductPreviewDTO> FindProductPreviews(string pictureBaseUrl)
        {
           //blazingly fast
           //5.4 i take it back, it would be better in one query, but im lazy
            var productPictures = context.ProductPictures
                .Where(p => p.IsPreview);

            var productRatings = context.Ratings
                .GroupBy(r => r.ProductID)
                .Select(g => new
                {
                    ProductID = g.Key,
                    AvgRating = (int?)g.Average(r => r.Score)
                });

            
            var query = this.context.Products
                .Select(product => new ProductPreviewDTO
                {
                    ID = product.ProductID,
                    Name = product.Name,
                    Discount = product.Discount,
                    Price = product.Price,
                    ImageUrl = productPictures
                        .Where(p => p.ProductID == product.ProductID)
                        .Select(p => $"{pictureBaseUrl}{p.PicturePath}")
                        .FirstOrDefault(),
                    Rating = productRatings
                        .Where(r => r.ProductID == product.ProductID)
                        .Select(r => r.AvgRating)
                        .FirstOrDefault() ?? RatingsService.DEFAULT_RATING,
                    CategoryID = product.CategoryId
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
