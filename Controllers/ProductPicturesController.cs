using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO.Pipes;
using WebApplication1.Models;
using WebApplication1.Models.DTO.Products;
using System.IO;
using WebApplication1.Services;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductPicturesController : ControllerBase
    {
        private MyContext context = new MyContext();
        private ProductPicturesService service = new ProductPicturesService();

        [HttpGet("{productID}/Preview")]
        public IActionResult GetPreviewPicturePath(int productID)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            var picURL = service.GetProductPreviewPicturePath(productID, baseUrl);

            if (picURL == null)
            {
                return NotFound(new { message = "Product preview picture not found" });
            }

            return Ok(picURL);


        }
        [HttpGet("{productID}")]
        public IActionResult GetProductPictures(int productID)
        {
            List<string> result = new List<string>();
            var pictures = context.ProductPictures.Where(picture => picture.ProductID == productID).ToList();

            if (pictures.Count == 0)
            {
                return NotFound(new { message = "Product doesn't exist or has no pictures" });
            }

            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            foreach (var item in pictures)
            {
                result.Add($"{baseUrl}{item.PicturePath}");
            }

            return Ok(result);
        }

        [HttpDelete("Delete:{pictureID}")]
        public bool DeleteProductPicture(int pictureID)
        {
            var pic = context.ProductPictures.Find(pictureID);
            if (pic == null)
            {
                return false;
            }

            this.service.DeletePictureFile(pic.PicturePath);

            this.context.ProductPictures.Remove(pic);
            this.context.SaveChanges();

            return true;
        }

        [HttpPost("Upload-image")]

        public IActionResult UploadImage(ProductPictureUploadDTO pic)
        {

            var fileName = Path.GetFileName(pic.File.FileName);
            var relativePath = $"/images/products/{pic.ProductId}/{fileName}";

            var validation = this.service.CanBeUploaded(pic, relativePath);

            if (!validation.Succeeded)
            {
                return BadRequest(validation.Message);
            }


            if (!this.service.SavePictureFile(pic))
            {
                return BadRequest("File isn't allowed");
            }

            ProductPicture picture = new ProductPicture
            {
                ProductID = pic.ProductId,
                PicturePath = relativePath,
                Description = pic.Description,
                IsPreview = pic.IsPreview
            };

            this.context.ProductPictures.Add(picture);
            this.context.SaveChanges();

            return Ok(new { imagePath = relativePath });
        }

    }
}
