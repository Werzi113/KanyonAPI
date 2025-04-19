using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Ratings")]
    public class Rating
    {
        public int RatingID { get; set; }
        public int ProductID { get; set; }
        public int UserID { get; set; }
        [Column("Rating")]
        public int Score { get; set; }
        public string? Description { get; set; }


    }
}
