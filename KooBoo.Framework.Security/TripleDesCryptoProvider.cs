using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KooBoo.Framework.Security
{
    public static class TripleDesCryptoProvider
    {
        /// <summary>
        /// Author: Des van Vuuren
        /// 
        /// Encrypts a string
        /// </summary>
        public static string Encrypt(string input, string password)
        {
            if (input != null && input != "")
            {
                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(input);

                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(password));

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }

            return "";
        }

        /// <summary>
        /// Author: Des van Vuuren
        /// 
        /// Decrypts a string
        /// </summary>
        public static string Decrypt(string input, string password)
        {
            if (input != null && input != "")
            {
                byte[] keyArray;
                byte[] toEncryptArray = Convert.FromBase64String(input);

                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(password));

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return UTF8Encoding.UTF8.GetString(resultArray);
            }

            return "";
        }
    }
}
