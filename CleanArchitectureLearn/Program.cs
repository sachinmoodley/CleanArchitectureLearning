using System;
using Domain.Requests;
using Domain.UseCases;
using Gateway;

namespace CleanArchitectureLearn
{
    partial class Program
    {
        private static UserInMemoryGateway _usersInMemoryGateway = new UserInMemoryGateway();
        private static UserGateway _userGateway = new UserGateway();
        private static MessageGateway _messageGateway = new MessageGateway();
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("" +
                                  "Enter 1 to create user || " +
                                  "2 to display all || " +
                                  "3 to delete || " +
                                  "4 to find a specific user || " +
                                  "5 to update a user || " +
                                  "6 to reset password");
                var response = Console.ReadLine();

                if (response == "1")
                {
                    AddUser();
                } 
                else if (response == "2")
                {
                    DisplayAllUsers();
                }

                else if (response == "3")
                {
                    DeleteUser();
                }

                else if (response == "4")
                {
                    GetSpecificUserById();
                }

                else if (response == "5")
                {
                    UpdateSpecificUserById();
                }

                else if (response == "6")
                {
                    ResetPassword();
                }
                else
                {
                    break;
                }
            }
        }

        private static void AddUser()
        {
            Console.WriteLine("Enter User Name");
            var newName = Console.ReadLine();
            Console.WriteLine("Enter Surname");
            var newSurname = Console.ReadLine();
            Console.WriteLine("Enter Email Address");
            var emailAddress = Console.ReadLine();

            Console.WriteLine("Enter Password");
            var password = Console.ReadLine();
            Console.WriteLine("Confirm Password");
            var confirmPassword = Console.ReadLine();

            var request = new CreateUserRequest
            {
                Name = newName,
                Surname = newSurname,
                EmailAddress = emailAddress,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            var useCase = new CreateUserUseCase(_userGateway);
            useCase.Execute(request, new PrefixedConsolePresenter("Added"));
        }

        private static void DisplayAllUsers()
        {
            foreach (var user in _userGateway.GetAll())
            {
                Console.WriteLine($"Id: {user.Id} , Name: {user.Name}, Email Address: {user.EmailAddress}");
            }
        }

        private static void DeleteUser()
        {
            Console.WriteLine("Enter user ID to delete");
            var idToDelete = Console.ReadLine();
            var request = new DeleteUserRequest
            {
                Id = idToDelete
            };
            var useCase = new DeleteUserUseCase(_userGateway);
            useCase.Execute(request, new PrefixedConsolePresenter("Deleted"));
        }

        private static void GetSpecificUserById()
        {
            Console.WriteLine("Enter ID to find specific user");
            var userId = Console.ReadLine();
            var request = new GetUserDetailsRequest
            {
                Id = userId
            };
            var useCase = new GetUserDetailsUseCase(_userGateway);
            useCase.Execute(request, new PrefixedConsolePresenter("Get"));
        }

        private static void UpdateSpecificUserById()
        {
            Console.WriteLine("Enter ID to find specific user to update");
            var userId = Console.ReadLine();
            Console.WriteLine("Enter updated name");
            var updatedName = Console.ReadLine();
            Console.WriteLine("Enter updated surname");
            var updatedSurname = Console.ReadLine();
            Console.WriteLine("Enter updated email address");
            var updatedEmail = Console.ReadLine();

            var request = new UpdateUserRequest
            {
                UserId = userId,
                Email = updatedEmail,
                Name = updatedName,
                Surname = updatedSurname
            };
            var useCase = new UpdateUserUseCase(_userGateway);
            useCase.Execute(request, new PrefixedConsolePresenter("Update"));
        }

        private static void ResetPassword()
        {
            Console.WriteLine("Enter email address");
            var email = Console.ReadLine();

            var getOtpRequest = new CreateOtpRequest
            {
                Email = email
            };
            var getOtpUseCase = new CreateOtpUseCase(_userGateway, _messageGateway);
            getOtpUseCase.Execute(getOtpRequest, new PrefixedConsolePresenter("Otp"));

            Console.WriteLine("Enter OTP");
            var userOtp = Console.ReadLine();
            Console.WriteLine("Enter new Password");
            var newPassword = Console.ReadLine();
            Console.WriteLine("Confirm new Password");
            var confirmPassword = Console.ReadLine();

            var request = new ResetPasswordRequest
            {
                Email = email,
                Otp = Convert.ToInt32(userOtp),
                NewPassword = newPassword,
                ConfirmPassword = confirmPassword
            };
            var useCase = new ResetPasswordUseCase(_userGateway);
            useCase.Execute(request, new PrefixedConsolePresenter("Reset"));
        }
    }
}
