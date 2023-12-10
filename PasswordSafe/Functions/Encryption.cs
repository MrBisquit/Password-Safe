using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordSafe.Functions
{
    public static class Encryption
    {
        public static byte[] GenerateKey(string input, int saltSize = 16, int iterations = 10000, int keySize = 256 / 8)
        {
            byte[] salt = new byte[saltSize];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(input, salt, iterations))
            {
                byte[] key = pbkdf2.GetBytes(keySize);
                byte[] result = new byte[keySize + saltSize];
                Buffer.BlockCopy(salt, 0, result, 0, saltSize);
                Buffer.BlockCopy(key, 0, result, saltSize, keySize);
                return result;
            }
        }

        // Modified, original from: https://remy.supertext.ch/2011/01/simple-c-encryption-and-decryption/
        public static string Encrypt(string source, byte[] key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] Key = key;
            byte[] IV = key;
            ICryptoTransform encryptor = des.CreateEncryptor(Key, IV);

            try
            {
                byte[] IDToBytes = ASCIIEncoding.ASCII.GetBytes(source);
                byte[] encryptedID = encryptor.TransformFinalBlock(IDToBytes, 0, IDToBytes.Length);
                return Convert.ToBase64String(encryptedID);
            }
            catch (FormatException)
            {
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static string Decrypt(string encrypted, byte[] key)
        {
            byte[] Key = key;
            byte[] IV = key;

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            ICryptoTransform decryptor = des.CreateDecryptor(Key, IV);

            try
            {
                byte[] encryptedIDToBytes = Convert.FromBase64String(encrypted);
                byte[] IDToBytes = decryptor.TransformFinalBlock(encryptedIDToBytes, 0, encryptedIDToBytes.Length);
                return ASCIIEncoding.ASCII.GetString(IDToBytes);
            }
            catch (FormatException)
            {
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
