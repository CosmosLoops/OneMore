﻿using Cosmos.Security.Cryptography;
using Cosmos.Security.Encryption.Abstractions;
using Cosmos.Security.Encryption.Algorithms;
using Xunit;

namespace Algorithms
{
    public class MonoalphabeticTests
    {
        readonly ICryptoAlgorithm _target = new Monoalphabetic();

        [Fact]
        public void EncryptTest()
        {
            //Arrange
            string plain = "abcd";

            //Act
            string cypher = _target.Encrypt(plain);

            //Assert
            Assert.NotEqual(cypher, plain);

            //Act
            string actual = _target.Decrypt(cypher);

            //Assert
            Assert.Equal(plain, actual);
        }
    }
}