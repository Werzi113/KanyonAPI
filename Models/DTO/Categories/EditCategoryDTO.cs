using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Models.DTO.Categories
{
    public class EditCategoryDTO
    {
        [Required()]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(65535)]
        public string? Description { get; set; }

        //public int? ParentID { get; set; }
    }
}