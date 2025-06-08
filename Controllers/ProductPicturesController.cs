using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO.Pipes;
using WebApplication1.Models;
using WebApplication1.Models.DTO.Products;
using System.IO;
using WebApplication1.Services;
using WebApplication1.Models.DTO.ProductPictures;


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
        public IActionResult GetProductPicturesPaths(int productID)
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
        [HttpGet("{productID}/AdminPreviews")]
        public IActionResult GetProductPicturesAdminPreviews(int productID)
        {
            var pictures = this.context.ProductPictures.Where(pic => pic.ProductID == productID);
            if (pictures.Count() < 1)
            {
                return Ok(pictures);
            }
            else
            {
                return Ok(pictures.Select(picture => new ProductPictureDTO()
                {
                    PictureID = picture.PictureID,
                    ProductID = picture.ProductID,
                    IsPreview = picture.IsPreview,
                    URL = $"{Request.Scheme}://{Request.Host}{picture.PicturePath}"
                }).ToList());
            }
        }
        [HttpPut("SetPreview:{pictureID}")]
        public IActionResult SetPreviewPicture(int pictureID)
        {
            var pic = context.ProductPictures.Find(pictureID);
            if (pic == null)
            {
                return NotFound(new { message = "Picture not found" });
            }
            if (pic.IsPreview)
            {
                return BadRequest(new { message = "This picture is already a preview" });
            }
            var currentPreview = context.ProductPictures.FirstOrDefault(p => p.ProductID == pic.ProductID && p.IsPreview);
            if (currentPreview != null)
            {
                currentPreview.IsPreview = false;
            }
            pic.IsPreview = true;
            context.SaveChanges();
            return Ok(new { message = "Preview picture updated successfully" });
        }

        [HttpDelete("Delete:{pictureID}")]
        public IActionResult DeleteProductPicture(int pictureID)
        {
            var pic = context.ProductPictures.Find(pictureID);
            if (pic == null)
            {
                return BadRequest(new { Message = "No picture found" });
            }

            this.service.DeletePictureFile(pic.PicturePath);

            this.context.ProductPictures.Remove(pic);
            this.context.SaveChanges();

            return Ok(new { Messsage = "Picture deleted successfully!" });
        }
        [HttpPost("Upload-images")]
        public IActionResult UploadImages(ProductPictureUploadDTO[] dto)
        {
            ProductPicturesService service = new ProductPicturesService();
            
            foreach (var item in dto)
            {
                var validationResult = service.CanBeUploaded(item);
                if (!validationResult.Succeeded)
                {
                    return BadRequest(validationResult.Message);
                }
                service.SavePicture(item, out string relativePath);

                ProductPicture picture = new ProductPicture()
                {
                    PicturePath = $"\\{relativePath}",
                    ProductID = item.ProductId,
                    Description = "",
                    IsPreview = false,
                };
                this.context.ProductPictures.Add(picture);
            }
            this.context.SaveChanges();



            return Ok(new { message = "Pictures uploaded successfully" });
        }
    }
}
