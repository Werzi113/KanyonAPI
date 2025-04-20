using System.ComponentModel.DataAnnotations;
using WebApplication1.Enums;

namespace WebApplication1.Models.DTO.Login
{
    public class EmailLoginDTO
    {
        [Required()]
        [MinLength(5)]
        [StringLength(100)]
        public string Password { get; set; }

        [Required()]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }
    }
}