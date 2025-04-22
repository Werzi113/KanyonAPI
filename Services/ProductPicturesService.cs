using WebApplication1.Models.DTO.Products;
using System.IO;

namespace WebApplication1.Services
{
    public class ProductPicturesService
    {
        private readonly string[] allowedFileSuffixes = new string[]
        {
            ".jpeg",
            ".jpg",
            ".png",
            ".jfif"
        };
        public bool SavePictureFile(ProductPictureUploadDTO pic)
        {
            var extension = Path.GetExtension(pic.File.Name);
            if (!allowedFileSuffixes.Contains(extension))
            {
                return false;
            }
            
            var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", pic.ProductId.ToString());
            if (!Directory.Exists(imageFolder))
            {
                Directory.CreateDirectory(imageFolder);
            }
            
            var fileName = Path.GetFileName(pic.File.FileName);
            var filePath = Path.Combine(imageFolder, fileName);

            

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                pic.File.CopyTo(stream);
            }

            return true;
        }

        public void DeletePictureFile(string filePath)
        {
            string root = Directory.GetCurrentDirectory();
            File.Delete(Path.Combine(root, "wwwroot", filePath));
        }
    }
}
