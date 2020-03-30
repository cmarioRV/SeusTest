using System;
using System.Security.Cryptography;

namespace demoseusapp
{
    public class SHA256Cryptography : Cryptography
    {
        protected override string EncryptValue(byte[] value)
        {
            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] hashValue = mySHA256.ComputeHash(value);
                return Convert.ToBase64String(hashValue).TrimEnd(Padding).Replace('+', '-').Replace('/', '_');
            }
        }
    }
}
