﻿namespace WebApplication1.Models
{
    public class User
    {
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
