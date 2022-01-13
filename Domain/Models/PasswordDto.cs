using System;

namespace Domain.Models
{
    public class PasswordDto
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }
    }
}