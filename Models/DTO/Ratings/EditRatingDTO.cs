using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Models.DTO.Ratings
{
    public class EditRatingDTO
    {
        [StringLength(65535)]
        public string? Description { get; set; }

        [Required()]
        [Range(1,5)]
        public int Score { get; set; }
    }
}