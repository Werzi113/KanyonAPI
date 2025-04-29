using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.DTO.Users
{
    public class CreateUserDTO
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

        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? LastName { get; set; }

        [Phone]
        [StringLength(16)]
        public string? PhoneNumber { get; set; }

        [StringLength(100)]
        [MinLength(1)]
        public string? City { get; set; }

        [StringLength(100)]
        [MinLength(1)]
        public string? Street { get; set; }

        [StringLength(100)]
        [MinLength(1)]
        public string? Country { get; set; }

        [StringLength(6)]
        [RegularExpression(@"^\d{3}\s?\d{2}$", ErrorMessage = "Wrong postcode format")]
        public string? PostCode { get; set; }

        public User CreateUser()
        {
            User user = new User();
            user.UserID = 0;
            user.Username = this.Username;
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(this.Password);
            user.Email = this.Email;
            user.CreatedAt = DateTime.Now;
            user.FirstName = this.FirstName;
            user.LastName = this.LastName;
            user.PhoneNumber = this.PhoneNumber;
            user.City = this.City;
            user.Street = this.Street;
            user.Country = this.Country;
            user.PostCode = this.PostCode;

            return user;
        }
    }
}