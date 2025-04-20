using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.DTO.Users
{
    public class PasswordOnlyDTO
    {
        [Required()]
        [StringLength(100)]
        [MinLength(5)]
        public string Password { get; set; }
    }
}