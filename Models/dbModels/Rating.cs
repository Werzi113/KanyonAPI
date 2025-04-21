using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.dbModels
{
    [Table("Ratings")]
    public class Rating
    {
        [Key]
        public int RatingID { get; set; }
        public int ProductID { get; set; }
        public int UserID { get; set; }
        [Column("Rating")]
        public int Score { get; set; }
        public string? Description { get; set; }


    }
}
