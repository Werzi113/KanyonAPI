using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("UserRights")]
    [PrimaryKey(nameof(UserID), nameof(Right))]
    public class UserRight
    {
        public int UserID { get; set; }
        public string Right { get; set; }

    }
}
