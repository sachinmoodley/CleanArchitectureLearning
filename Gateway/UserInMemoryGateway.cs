using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Domain.Interfaces;
using Domain.Models;

namespace Gateway
{
    public class UserInMemoryGateway : IUserGateway
    {
        private static List<User> users = new List<User>();
        private static List<PasswordDto> passwords = new List<PasswordDto>();
        private static List<UserOtp> userOtp = new List<UserOtp>();
        
        public void Add(User user)
        {
            users.Add(user);
        }

        public List<User> GetAll()
        {
            return users;
        }

        public void Delete(User user)
        {
            users.Remove(user);
        }

        public User Get(Guid id)
        {
            return users.FirstOrDefault(x => x.Id == id);
        }

        public User Update(User updatedUser)
        {
            var userToUpdate = users.FirstOrDefault(x => x.Id == updatedUser.Id);
            if (userToUpdate != null)
            {
                userToUpdate.Name = updatedUser.Name;
                userToUpdate.Surname = updatedUser.Surname;
                userToUpdate.EmailAddress = updatedUser.EmailAddress;
                return updatedUser;
            }

            throw new Exception("Cannot find user");
        }

        public void AddPassword(PasswordDto password)
        {
            passwords.Add(password);
        }

        public User GetUserBy(string email)
        {
            return users.FirstOrDefault(x => x.EmailAddress == email);
        }

        public void StoreOtp(Guid id, int otp)
        {
            var existingUserOtp = userOtp.FirstOrDefault(x => x.Id == id);
            if (existingUserOtp != null)
            {
                if (existingUserOtp.Otp != otp)
                {
                    existingUserOtp.Otp = otp;
                }
            }

            userOtp.Add(new UserOtp
            {
                Id = id,
                Otp = otp
            });
        }

        public bool HasValidOtp(Guid id, int otp)
        {
            return userOtp.Any(x => x.Id == id && x.Otp == otp);
        }

        public void UpdatePassword(Guid id, string newPassword, string confirmPassword)
        {
            var userPassword = passwords.FirstOrDefault(x => x.Id == id);
            if (userPassword != null)
            {
                var salt = GetSalt();
                userPassword.Password = newPassword;
                userPassword.ConfirmPassword = confirmPassword;
                userPassword.Hash = GetHash(newPassword + salt);
                userPassword.Salt = salt;
            }
        }

        private static string GetHash(string text)
        {
            // SHA512 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private static string GetSalt()
        {
            byte[] bytes = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}