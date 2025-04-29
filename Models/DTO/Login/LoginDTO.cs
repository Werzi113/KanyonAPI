using System.ComponentModel.DataAnnotations;
using WebApplication1.Enums;

namespace WebApplication1.Models.DTO.Login
{
    public class LoginDTO
    {
        [Required()]
        [MinLength(5)]
        [StringLength(100)]
        public string Password { get; set; }

        [Required()]
        [StringLength(100)]
        public string Username { get; set; }
    }
}