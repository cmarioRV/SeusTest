using System;
using System.Text;

namespace demoseusapp
{
    public abstract class Cryptography : ICryptography
    {
        protected readonly char[] Padding = { '=' };

        public string EncryptValue(string value)
        {
            return EncryptValue(Encoding.UTF8.GetBytes(value));
        }

        public string GenerateRandomValue(int lenght)
        {
            var random = new Random();
            var bytes = new byte[lenght];
            random.NextBytes(bytes);
            return Convert.ToBase64String(bytes).TrimEnd(Padding).Replace('+', '-').Replace('/', '_');
        }

        protected abstract string EncryptValue(byte[] value);
    }
}
