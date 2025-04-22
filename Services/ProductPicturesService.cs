using WebApplication1.Models.DTO.Products;
using System.IO;
using WebApplication1.Models;
using Stripe;

namespace WebApplication1.Services
{
    public class ProductPicturesService
    {
        private MyContext context = new MyContext();
        private readonly string[] allowedFileSuffixes = new string[]
        {
            ".jpeg",
            ".jpg",
            ".png",
            ".jfif"
        };
        public bool SavePictureFile(ProductPictureUploadDTO pic)
        {
            var extension = Path.GetExtension(pic.File.FileName);
            if (!allowedFileSuffixes.Contains(extension))
            {
                return false;
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", pic.ProductId.ToString());
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fileName = Path.GetFileName(pic.File.FileName);
            var filePath = Path.Combine(path, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                pic.File.CopyTo(stream);
            }

            return true;
        }

        public void DeletePictureFile(string filePath)
        {
            string root = Directory.GetCurrentDirectory();
            System.IO.File.Delete(Path.Combine(root, "wwwroot", filePath));
        }

        public ValidationResult CanBeUploaded(ProductPictureUploadDTO pic, string path)
        {
            ValidationResult result = new ValidationResult()
            {
                Succeeded = false
            };

            if (pic.File == null || pic.File.Length == 0)
            {
                result.Message = "No file uploaded";
                return result;
            }
            if (this.context.ProductPictures.Any(img => img.PicturePath == path))
            {
                result.Message = "Provided picture already exists for this product";
                return result;
            }
            if (this.context.Products.Find(pic.ProductId) == null)
            {
                result.Message = $"Product with id {pic.ProductId} doesn't exist";
                return result;
            }

            result.Succeeded = true;
            return result;
        }

    }
}
