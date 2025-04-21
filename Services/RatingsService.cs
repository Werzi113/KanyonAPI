namespace WebApplication1.Services
{
    public class RatingsService
    {
        private MyContext _context = new MyContext();
        public int GetProductRating(int productId)
        {
            try
            {
                return (int)this._context.Ratings.Where(rating => rating.ProductID == productId).Average(rating => rating.Score);
            }
            catch
            {
                return 0;
            }
        }

    }
}
