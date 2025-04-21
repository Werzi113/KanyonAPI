using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Models.DTO.Ratings
{
    public class CreateRatingAdminDTO
    {
        [Required()]
        public int UserID { get; set; }

        [Required()]
        public int ProductID { get; set; }

        [StringLength(65535)]
        public string? Description { get; set; }

        [Required()]
        [Range(1,5)]
        public int Score { get; set; }

        public Rating CreateRating()
        {
            Rating rating = new Rating();

            rating.UserID = UserID;
            rating.ProductID = ProductID;
            rating.Description = Description;
            rating.Score = Score;

            return rating;
        }
    }
}