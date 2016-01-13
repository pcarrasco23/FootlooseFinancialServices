using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Service
{
    public static class PasswordUtils
    {
        public static string CreateSalt(int size)
        {
            return string.Empty;
        }

        public static string GenerateHashedPassword(string password, string salt)
        {
            return password;
        }

        public static OperationStatus ValidatePassword(string password)
        {
            return new OperationStatus { Success = true, Messages = null };
        }
    }
}
