using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Domain.Interfaces;
using Domain.Models;

namespace Gateway
{
    //ToDo : refactor into entity framework
    public class UserGateway : IUserGateway
    {
        public void Add(User user)
        {
            var command = $"INSERT INTO [dbo].[User] (Id, Name, Surname, EmailAddress) VALUES('{user.Id}', '{user.Name}', '{user.Surname}','{user.EmailAddress}')";
            ExecuteSqlCommand(command);
        }

        public List<User> GetAll()
        {
            string connString = @"Data Source=localhost;Initial Catalog=CleanArchitectureLearn;Integrated Security=True";
            try
            {
                //sql connection object
                using (SqlConnection conn = new SqlConnection(connString))
                {

                    //retrieve the SQL Server instance version
                    string query = @"SELECT u.Id,u.Name,u.Surname,u.EmailAddress
                                     FROM [dbo].[User] u";
                    //define the SqlCommand object
                    SqlCommand cmd = new SqlCommand(query, conn);

                    //open connection
                    conn.Open();

                    //execute the SQLCommand
                    SqlDataReader dr = cmd.ExecuteReader();

                    Console.WriteLine(Environment.NewLine + "Retrieving data from database..." + Environment.NewLine);
                    Console.WriteLine("Retrieved records:");

                    //check if there are records
                    if (dr.HasRows)
                    {
                        var rows = new List<User>();
                        while (dr.Read())
                        {
                            var model = new User
                            {
                                Id = Guid.Parse(dr.GetString(0)),
                                Name = dr.GetString(1),
                                Surname = dr.GetString(2),
                                EmailAddress = dr.GetString(3)
                            };
                            rows.Add(model);
                        }

                        return rows;
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                    }

                    //close data reader
                    dr.Close();

                    //close connection
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            return new List<User>();
        }

        public void Delete(User user)
        {
            var command = $"DELETE FROM [dbo].[User] where Id = '{user.Id}'";
            ExecuteSqlCommand(command);
        }

        public User Get(Guid id)
        {
            string connString = @"Data Source=localhost;Initial Catalog=CleanArchitectureLearn;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {

                    var query = $"SELECT TOP 1 u.Id,u.Name,u.Surname,u.EmailAddress FROM [dbo].[User] u where u.Id = '{id}'";
                    var cmd = new SqlCommand(query, conn);
                    conn.Open();
                    var dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var model = new User
                            {
                                Id = Guid.Parse(dr.GetString(0)),
                                Name = dr.GetString(1),
                                Surname = dr.GetString(2),
                                EmailAddress = dr.GetString(3)
                            };

                            return model;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                    }

                    dr.Close();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            return new User();
        }

        public User Update(User updatedUser)
        {
            var command = $"UPDATE [dbo].[User] SET Name = '{updatedUser.Name}', Surname = '{updatedUser.Surname}', EmailAddress = '{updatedUser.EmailAddress}' WHERE Id = '{updatedUser.Id}'";
            ExecuteSqlCommand(command);

            return updatedUser;
        }

        public void AddPassword(PasswordDto password)
        {
            var salt = GetSalt();
            var command = $"INSERT INTO [dbo].[Password] (Id, Password, ConfirmPassword, Hash, Salt) VALUES('{password.Id}', '{password.Password}', '{password.ConfirmPassword}','{GetHash(password.Password + salt)}', '{salt}')";
            ExecuteSqlCommand(command);
        }

        public User GetUserBy(string email)
        {
            string connString = @"Data Source=localhost;Initial Catalog=CleanArchitectureLearn;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {

                    var query = $"SELECT TOP 1 u.Id,u.Name,u.Surname,u.EmailAddress FROM [dbo].[User] u where u.EmailAddress = '{email}'";
                    var cmd = new SqlCommand(query, conn);
                    conn.Open();
                    var dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var model = new User
                            {
                                Id = Guid.Parse(dr.GetString(0)),
                                Name = dr.GetString(1),
                                Surname = dr.GetString(2),
                                EmailAddress = dr.GetString(3)
                            };

                            return model;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                    }

                    dr.Close();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            return new User();
        }

        public void StoreOtp(Guid id, int otp)
        {
            string connString = @"Data Source=localhost;Initial Catalog=CleanArchitectureLearn;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    var query = $"select top 1 * from dbo.UserOtp where id = '{id}'";
                    var cmd = new SqlCommand(query, conn);
                    conn.Open();
                    var dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Close();
                        //then  update
                        query = $"UPDATE [dbo].[UserOtp] SET Otp = '{otp}' WHERE Id = '{id}'";
                        var cmd2 = new SqlCommand(query, conn);
                        var dr2 = cmd2.ExecuteReader();
                        dr2.Close();
                        conn.Close();
                        return;
                    }

                    dr.Close();
                    query = $"INSERT INTO [dbo].[UserOtp] (Id, Otp) VALUES('{id}', '{otp}')";
                    var cmd3 = new SqlCommand(query, conn);
                    var dr3 = cmd3.ExecuteReader();
                    dr3.Close();

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public bool HasValidOtp(Guid id, int otp)
        {
            string connString = @"Data Source=localhost;Initial Catalog=CleanArchitectureLearn;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {

                    var query = $"select top 1 * from dbo.UserOtp where id = '{id}' AND otp = '{otp}'";
                    var cmd = new SqlCommand(query, conn);
                    conn.Open();
                    var dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Close();
                        conn.Close();
                        return true;
                    }

                    dr.Close();
                    conn.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            return false;
        }

        public void UpdatePassword(Guid id, string newPassword, string confirmPassword)
        {
            var salt = GetSalt();
            var command = $"UPDATE [dbo].[Password] SET Password = '{newPassword}', ConfirmPassword = '{confirmPassword}', Hash = '{ GetHash(newPassword + salt)}', Salt = '{salt}' WHERE Id = '{id}'";
            ExecuteSqlCommand(command);
        }

        private void ExecuteSqlCommand(string sqlCommand)
        {
            SqlConnection sqlConnection1 =
                new SqlConnection("Data Source=localhost;Initial Catalog=CleanArchitectureLearn;Integrated Security=True");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = sqlCommand;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();
            cmd.ExecuteNonQuery();
            sqlConnection1.Close();
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