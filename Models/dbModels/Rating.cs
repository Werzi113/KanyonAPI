using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Ratings")]
    [PrimaryKey(nameof(UserID), nameof(ProductID))]
    public class Rating
    {
        public int ProductID { get; set; }
        public int UserID { get; set; }
        [Column("Rating")]
        public int Score { get; set; }
        public string? Description { get; set; }


    }
}