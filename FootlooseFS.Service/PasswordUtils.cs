using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
            return new OperationStatus { Success = true, Messages = null };
        }
    }
}
