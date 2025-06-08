namespace WebApplication1.Models.DTO.ProductPictures
{
    public class ProductPictureDTO
    {
        public int PictureID { get; set; }
        public int ProductID { get; set; }
        public string URL { get; set; }
        public bool IsPreview { get; set; } 
    }
}
