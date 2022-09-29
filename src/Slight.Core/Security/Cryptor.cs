using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Slight.Core
{
    public static class Cryptor
    {
        public static byte[] Encrypt(string encryptKey, byte[] originBuffer)
        {

            string keyBuffer = MD5(encryptKey).Substring(0, 8);
            byte[] buffer = Encoding.UTF8.GetBytes(keyBuffer);
            using DESCryptoServiceProvider des = new()
            {
                Key = buffer,
                IV = buffer
            };
            using MemoryStream ms = new();
            using CryptoStream cryptoStream = new(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(originBuffer, 0, originBuffer.Length);
            cryptoStream.FlushFinalBlock();
            buffer = ms.ToArray();

            return buffer;

        }

        public static byte[] Decrypt(string encryptKey, byte[] encryptBuffer)
        {

            string keyBuffer = MD5(encryptKey).Substring(0, 8);
            byte[] buffer = Encoding.UTF8.GetBytes(keyBuffer);
            using DESCryptoServiceProvider des = new()
            {
                Key = buffer,
                IV = buffer
            };

            MemoryStream ms = new(encryptBuffer);
            CryptoStream cryptoStream = new(ms, des.CreateDecryptor(), CryptoStreamMode.Read);

            buffer = new byte[200];
            MemoryStream ms_temp = new();

            int i = 0;
            while ((i = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms_temp.Write(buffer, 0, i);
            }
            return ms_temp.ToArray();


        }


        public static string MD5(string @string)
        {

            byte[] b = Encoding.UTF8.GetBytes(@string);
            b = new MD5CryptoServiceProvider().ComputeHash(b);

            StringBuilder sb = new();
            for (int i = 0; i < b.Length; i++)
            {
                string bf = b[i].ToString("x2");
                sb.Append(bf);
            }

            return sb.ToString();
        }
    }
}
