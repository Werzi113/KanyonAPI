using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Models.DTO.Categories
{
    public class CreateCategoryDTO
    {
        [Required()]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(65535)]
        public string? Description { get; set; }

        public int? ParentID { get; set; }

        public Category CreateCategory()
        {
            Category category = new Category();

            category.Name = Name;
            category.Description = Description;
            category.ParentID = ParentID;
            category.CategoryID = 0;

            return category;
        }
    }
}