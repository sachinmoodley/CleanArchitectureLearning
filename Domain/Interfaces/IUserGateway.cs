using System;
using System.Collections.Generic;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IUserGateway
    {
        void Add(User user);

        List<User> GetAll();
        void Delete(User user);
        User Get(Guid id);
        User Update(User updatedUser);
        void AddPassword(PasswordDto password);

        User GetUserBy(string email);
        void StoreOtp(Guid id, int otp);
        bool HasValidOtp(Guid id, int otp);
        void UpdatePassword(Guid id, string newPassword, string confirmPassword);
    }
}