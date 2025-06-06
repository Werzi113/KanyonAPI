using Microsoft.OpenApi.Expressions;
using WebApplication1.Models.DTO.Products;

namespace WebApplication1.Services
{
    public class FilterService
    {
        public IQueryable<ProductPreviewDTO> FilterByName(IQueryable<ProductPreviewDTO> query, string? name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                return query.Where(p => p.Name.Contains(name.Trim()));
            }
            return query;
        }
        public IQueryable<ProductPreviewDTO> FilterByPriceRange(IQueryable<ProductPreviewDTO> query, int? min, int? max)
        {
            if (min.HasValue && max.HasValue)
            {
                return query.Where(p => p.Price >= min && p.Price <= max);
            }
            return query;
        }
        public IQueryable<ProductPreviewDTO> FilterByCategory(IQueryable<ProductPreviewDTO> query, int? categoryID)
        {
            
            CategoryService s = new CategoryService();
            if (categoryID != null)
            {
                return query.Where(product => s.getAllChildrenOfCategory((int)categoryID).ToArray()
                            .Select(cat => cat.CategoryID)
                            .Contains(product.CategoryID));
            }
            return query;
        }
        public IQueryable<ProductPreviewDTO> FilterByRating(IQueryable<ProductPreviewDTO> query, int? minRating)
        {
            if (minRating.HasValue)
            {
                return query.Where(p => p.Rating >= minRating);
            }
            return query;
        }
    }
}
