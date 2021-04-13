﻿using Cosmos.Security.Cryptography;
using Cosmos.Security.Encryption.Abstractions;
using Cosmos.Security.Encryption.Algorithms;
using Xunit;

namespace Algorithms
{
    public class RowTranspositionTests
    {
        readonly ICryptoAlgorithm _target;

        public RowTranspositionTests()
        {
            _target = new RowTransposition(new int[] {4, 3, 1, 2, 5, 6, 7});
        }

        [Fact]
        public void EncryptTest()
        {
            //Arrange
            string plain = "attackpostponeduntiltwoam";
            string cypher = "ttna aptm tsuo aodw coi* knl* pet* ";

            //Act
            string actual = _target.Encrypt(plain);

            //Assert
            Assert.Equal(cypher, actual);
        }

        [Fact()]
        public void DecryptTest()
        {
            //Arrange
            string plain = "attackpostponeduntiltwoam";
            string cypher = "ttna aptm tsuo aodw coi* knl* pet* ";

            //Act
            string actual = _target.Decrypt(cypher);

            //Assert
            Assert.Equal(plain, actual);
        }
    }
}