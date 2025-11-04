using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Utils
{
    public static class AuthUtil
    {
        public static bool VerifyPassword(string userPassword, string password)
        {
            
            //var userPass = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(userPassword));
            //return userPass.Equals(password);
            return userPassword.Equals(password);
        }
    }
}
