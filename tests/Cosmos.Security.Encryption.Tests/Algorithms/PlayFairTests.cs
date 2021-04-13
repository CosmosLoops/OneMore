﻿using Cosmos.Security.Cryptography;
using Cosmos.Security.Encryption.Abstractions;
using Cosmos.Security.Encryption.Algorithms;
using Xunit;

namespace Algorithms
{
    public class PlayFairTests
    {
        readonly ICryptoAlgorithm _target;

        public PlayFairTests()
        {
            _target = new PlayFair("playfairexample");
        }

        [Fact]
        public void EncryptTest()
        {
            //Arrange
            string plain = "hidethegoldinthetreestump";
            string cypher = "bmodzbxdnabekudmuixmmouvif";

            //Act
            string actual = _target.Encrypt(plain);

            //Assert
            Assert.Equal(cypher, actual);
        }

        [Fact]
        public void DecryptTest()
        {
            //Arrange
            string plain = "hidethegoldinthetrexestump";
            string cypher = "bmodzbxdnabekudmuixmmouvif";

            //Act
            string actual = _target.Decrypt(cypher);

            //Assert
            Assert.Equal(plain, actual);
        }
    }
}