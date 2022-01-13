using System;

namespace Domain.Models
{
    public class UserOtp
    {
        public Guid Id { get; set; }
        public int Otp { get; set; }
    }
}