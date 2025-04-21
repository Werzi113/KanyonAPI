using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.dbModels
{
    [Table("ProductPictures")]
    public class ProductPicture
    {
        [Key]
        public int PictureID { get; set; }
        public int ProductID { get; set; }
        public string PicturePath { get; set; }
        public string? Description { get; set; }

    }
}
