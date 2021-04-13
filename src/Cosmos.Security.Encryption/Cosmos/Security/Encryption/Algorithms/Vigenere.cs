﻿using System;
using Cosmos.Security.Cryptography;
using Cosmos.Security.Encryption.Abstractions;
using Cosmos.Security.Encryption.Core.Internals;

namespace Cosmos.Security.Encryption.Algorithms
{
    /// <summary>
    /// Vigenere encryption algorithm
    /// for more info, please view:
    ///     https://www.codeproject.com/Articles/63432/Classical-Encryption-Techniques
    /// Author: Omar-Salem
    ///     https://github.com/Omar-Salem/Classical-Encryption-Techniques/blob/master/EncryptionAlgorithms/Concrete/Vigenere.cs
    /// </summary>
    public sealed class Vigenere : ICryptoAlgorithm
    {
        private string Key { get; }

        /// <summary>
        /// Create a new instance of <see cref="Vigenere"/>
        /// </summary>
        /// <param name="key"></param>
        public Vigenere(string key) => Key = key;

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string Encrypt(string plainText) => ProcessFunc()(Key)(plainText)(EncryptionAlgorithmMode.Encrypt);

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns></returns>
        public string Decrypt(string cipher) => ProcessFunc()(Key)(cipher)(EncryptionAlgorithmMode.Decrypt);

        private static Func<string, Func<string, Func<EncryptionAlgorithmMode, string>>> ProcessFunc() => key => message => mode =>
        {
            key = key.ToString().ToLower().Replace(" ", "");
            key = DuplicateKeyFunc()(key)(message);
            return AlgorithmUtils.Shift(message, key, mode, AlphabetDictionaryGenerator.Generate());
        };

        private static Func<string, Func<string, string>> DuplicateKeyFunc() => key => message =>
        {
            if (key.Length < message.Length)
            {
                var length = message.Length - key.Length;

                for (var i = 0; i < length; i++)
                {
                    key += key[i % key.Length];
                }
            }

            return key;
        };
    }
}