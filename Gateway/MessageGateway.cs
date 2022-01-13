using System;
using System.IO;
using Domain;
using Domain.Interfaces;

namespace Gateway
{
    public class MessageGateway : IMessageGateway
    {
        public void SendMessage(int otp)
        {
            string folder = @"C:\Users\sachin\source\repos\CleanArchitectureLearn\";
            string fileName = "otps.txt";
            string fullPath = folder + fileName;

            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                writer.WriteLine($"Your OTP is {otp}");
            }
        }
    }
}