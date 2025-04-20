using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.DTO.Users
{
    public class ChangeUniqueInfoDTO
    {
        [Required()]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required()]
        [StringLength(100)]
        [MinLength(5)]
        public string Username { get; set; }

        [Required()]
        [StringLength(100)]
        [MinLength(5)]
        public string Password { get; set; }
    }
}