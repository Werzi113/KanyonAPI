namespace WebApplication1.Models.DTO.Products
{
    public class ProductPictureUploadDTO
    {
        public IFormFile File { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public bool IsPreview { get; set; } = false;
    }
}
