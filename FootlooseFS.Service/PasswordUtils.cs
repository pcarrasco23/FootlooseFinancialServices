using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FootlooseFS.Service
{
    public static class PasswordUtils
    {
        public static string CreateSalt(int size)
        {
            // Using a cryptographic random number generator, create a salt and return
            // the salt in a base64 encoded string
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[size];

                rng.GetBytes(data);
                
                var salt = Convert.ToBase64String(data);

                return salt;
            }
        }

        public static string GenerateHashedPassword(string password, string salt)
        {
            // We will be using SHA256 cryptographic hash function
            HashAlgorithm hash = new SHA256Managed();

            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

            // Because the salt is stored in base-64 we just need to convert it back to 
            // an array of bytes
            byte[] saltBytes = Convert.FromBase64String(salt);
           
            var hashInput = new byte[passwordBytes.Length + saltBytes.Length];
            passwordBytes.CopyTo(hashInput, 0);
            saltBytes.CopyTo(hashInput, passwordBytes.Length);

            // Compute a hash based on the password + salt and convert to base64 string
            byte[] hashBytes = hash.ComputeHash(hashInput);
            string hashValue = Convert.ToBase64String(hashBytes);

            return hashValue;
        }

        public static OperationStatus ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return new OperationStatus { Success = false, Messages = new List<string> { "The password cannot be blank." } };
            }
            else if (password.IndexOf(" ") >= 0)
            {
                return new OperationStatus { Success = false, Messages = new List<string> { "The password cannot have any whitespace characters." } };
            }
            else if (password.Length < 8 || password.Length > 15)
            {
                return new OperationStatus { Success = false, Messages = new List<string> { "The password must be between 8 and 15 characters." } };
            }
            else
            {
                // The password must contain:
                // 1. At least one lowercase character
                // 2. At least one uppercaase character
                // 3. At least one digit
                // 4. At least one special character
                // 5. Must be between 8 and 15 characters long            
                Match lowercase = Regex.Match(password, @"^(?=.*[a-z])");
                Match uppercase = Regex.Match(password, @"^(?=.*[A-Z])");
                Match digit = Regex.Match(password, @"^(?=.*\d)");
                Match specialCharacter = Regex.Match(password, @"^(?=.*[^\da-zA-Z])");

                if (!lowercase.Success)
                {
                    return new OperationStatus { Success = false, Messages = new List<string> { "The password must contain atleast one lowercase character." } };
                }

                if (!uppercase.Success)
                {
                    return new OperationStatus { Success = false, Messages = new List<string> { "The password must contain atleast one upper character." } };
                }

                if (!digit.Success)
                {
                    return new OperationStatus { Success = false, Messages = new List<string> { "The password must contain atleast one digit." } };
                }

                if (!specialCharacter.Success)
                {
                    return new OperationStatus { Success = false, Messages = new List<string> { "The password must contain atleast one non-alphanumeric character." } };
                }

                return new OperationStatus { Success = true, Messages = null };                
            }            
        }
    }
}
