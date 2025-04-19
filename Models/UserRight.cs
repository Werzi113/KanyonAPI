using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("UserRights")]
    public class UserRight
    {
        public int UserID { get; set; }
        public string Right { get; set; }

    }
}
