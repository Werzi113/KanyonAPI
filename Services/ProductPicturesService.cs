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
        public string? GetProductPreviewPicturePath(int productID, string baseURL)
        {
            var previewPicture = context.ProductPictures.FirstOrDefault(p => p.IsPreview && p.ProductID == productID);

            return previewPicture == null ? null : $"{baseURL}{previewPicture.PicturePath}";
        }
        public bool SavePicture(ProductPictureUploadDTO pic, out string relativePath)
        {
            var parts = pic.ImageBase64.Split(',');
            string metadata = parts[0];
            string extension = "." + metadata.Split('/')[1].Split(';')[0];

            var fileName = $"product_" + pic.ProductId + "_" + DateTime.Now.Ticks + extension;

            // Složka pro obrázky produktu
            string folderRelative = Path.Combine("images", "products", pic.ProductId.ToString());
            string folderAbsolute = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderRelative);

            if (!Directory.Exists(folderAbsolute))
            {
                Directory.CreateDirectory(folderAbsolute);
            }

            relativePath = Path.Combine(folderRelative, fileName);
            string filePath = Path.Combine(folderAbsolute, fileName);

            byte[] fileBytes = Convert.FromBase64String(parts[1]);
            System.IO.File.WriteAllBytes(filePath, fileBytes);

            return true;
        }

        public void DeletePictureFile(string filePath)
        {
            string root = Directory.GetCurrentDirectory();
            System.IO.File.Delete(root + "\\wwwroot" + filePath);
        }
        public void DeleteProductPictures(int id)
        {
            string root = Directory.GetCurrentDirectory();
            foreach (var item in this.context.ProductPictures.Where(pic => pic.ProductID == id))
            {
                this.DeletePictureFile(item.PicturePath);
            }
            string path = Path.Combine(root, "wwwroot\\images\\products\\", id.ToString());

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

        }

        public ValidationResult CanBeUploaded(ProductPictureUploadDTO pic)
        {
            ValidationResult result = new ValidationResult()
            {
                Succeeded = false
            };
            string metadata;
            string extension;
            try
            {
                metadata = pic.ImageBase64.Split(',')[0];
                extension = "." + metadata.Split('/')[1].Split(';')[0];

            }
            catch
            {
                result.Message = "Wrong file format";
                return result;
            }


            if (!allowedFileSuffixes.Contains(extension))
            {
                result.Message = "File isn't allowed";
                return result;
            }
            if (string.IsNullOrEmpty(pic.ImageBase64))
            {
                result.Message = "No file uploaded";
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
