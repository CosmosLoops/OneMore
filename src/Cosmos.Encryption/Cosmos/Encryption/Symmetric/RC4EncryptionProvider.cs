﻿using System;
using System.Linq;
using System.Text;
using Cosmos.Encryption.Abstractions;
using Cosmos.Optionals;

// ReSharper disable once CheckNamespace
namespace Cosmos.Encryption
{
    /// <summary>
    /// Symmetric/RC4 encryption.
    /// Reference: https://bitlush.com/blog/rc4-encryption-in-c-sharp
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class RC4EncryptionProvider : ISymmetricEncryption
    {
        private RC4EncryptionProvider() { }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Encrypt(string data, string key, Encoding encoding = null)
        {
            encoding = encoding.SafeValue();
            return Convert.ToBase64String(EncryptCore(encoding.GetBytes(data), encoding.GetBytes(key)));
        }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Encrypt(byte[] data, string key, Encoding encoding = null)
        {
            encoding = encoding.SafeValue();
            return Convert.ToBase64String(EncryptCore(data, encoding.GetBytes(key)));
        }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, byte[] key)
        {
            return EncryptCore(data, key);
        }

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Decrypt(string data, string key, Encoding encoding = null)
        {
            encoding = encoding.SafeValue();
            return encoding.GetString(EncryptCore(Convert.FromBase64String(data), encoding.GetBytes(key)));
        }

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] data, byte[] key)
        {
            return EncryptCore(data, key);
        }

        private static byte[] EncryptCore(byte[] data, byte[] key)
        {
            var s = Initalize(key);
            int i = 0, j = 0;

            return data.Select(b =>
            {
                i = (i + 1) & 255;
                j = (j + s[i]) & 255;
                Swap(s, i, j);
                return (byte) (b ^ s[(s[i] + s[j]) & 255]);
            }).ToArray();
        }

        private static byte[] Initalize(byte[] key)
        {
            var s = Enumerable.Range(0, 256).Select(i => (byte) i).ToArray();
            for (int i = 0, j = 0; i < 256; i++)
            {
                j = (j + key[i % key.Length] + s[i]) & 255;
                Swap(s, i, j);
            }

            return s;
        }

        private static void Swap(byte[] s, int i, int j)
        {
            var b = s[i];
            s[i] = s[j];
            s[j] = b;
        }
    }
}