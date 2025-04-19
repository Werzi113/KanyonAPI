using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string Username {  get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt {  get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber {  get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? Country { get; set; }
        public string? PostCode {  get; set; }

    }
}
