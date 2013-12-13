using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Web;
using System.Web.Security;

namespace WooAuth.Utils
{
    public static class StringEncryptor
    {
        public static string Encrypt(this string plaintextValue)
        {
            var plaintextBytes = Encoding.UTF8.GetBytes(plaintextValue);
            return Convert.ToBase64String(MachineKey.Protect(plaintextBytes, "Authentication token"));
        }

        public static string Decrypt(this string encryptedValue)
        {
            try
            {
                var decryptedBytes = MachineKey.Unprotect(Convert.FromBase64String(encryptedValue), "Authentication token");
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch
            {
                throw new SecurityException("token forgery");
            }
        }
    }
}