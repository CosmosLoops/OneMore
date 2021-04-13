﻿using Cosmos.Security.Cryptography;
using Cosmos.Security.Encryption.Abstractions;
using Cosmos.Security.Encryption.Algorithms;
using Xunit;

namespace Algorithms {
    public class CeaserTests {
        readonly ICryptoAlgorithm _target;

        public CeaserTests() {
            _target = new Ceaser(3);
        }

        [Fact]
        public void EncryptTest() {
            //Arrange
            string plain = "meetmeafterthetogaparty";
            string cypher = "phhwphdiwhuwkhwrjdsduwb";

            //Act
            string actual = _target.Encrypt(plain);

            //Assert
            Assert.Equal(cypher, actual);
        }

        [Fact]
        public void DecryptTest() {
            //Arrange
            string plain = "meetmeafterthetogaparty";
            string cypher = "phhwphdiwhuwkhwrjdsduwb";

            //Act
            string actual = _target.Decrypt(cypher);

            //Assert
            Assert.Equal(plain, actual);
        }
    }
}