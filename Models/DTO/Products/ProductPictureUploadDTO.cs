namespace WebApplication1.Models.DTO.Products
{
    public class ProductPictureUploadDTO
    {
        public string ImageBase64 { get; set; }
        public int ProductId { get; set; }
        public bool IsPreview { get; set; } = false;
    }
}
