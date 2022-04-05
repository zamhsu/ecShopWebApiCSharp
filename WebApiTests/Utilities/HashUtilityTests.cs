using WebApi.Utilities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Utilities.Tests
{
    [TestClass()]
    public class HashUtilityTests
    {
        [TestMethod()]
        public void HashSha256_AreEqual()
        {
            // Arrange
            byte[] data = Encoding.UTF8.GetBytes("ABC123");
            GeneralHashAlgorithmPara hashAlgorithm = GeneralHashAlgorithmPara.SHA256;
            string expected = "E0BEBD22819993425814866B62701E2919EA26F1370499C1037B53B9D49C2C8A";

            // Act
            string actual = HashUtility.CreateGeneralHash(data, hashAlgorithm);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void HashSha512_AreEqual()
        {
            // Arrange
            byte[] data = Encoding.UTF8.GetBytes("ABC123");
            GeneralHashAlgorithmPara hashAlgorithm = GeneralHashAlgorithmPara.SHA512;
            string expected = "8C9333343C6C4222418EDB1D7C9F84D051610526085960A1732C7C3D763FFF64EC7F5220998434C896DDA243AE777D0FB213F36B9B19F7E4A244D5C993B8DFED";

            // Act
            string actual = HashUtility.CreateGeneralHash(data, hashAlgorithm);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), 
            "Must longer than 16 characters")]
        public void HmacSha256Signature_ArgumentException()
        {
            // Arrange
            string signKey = "ABC123";

            // Act
            SigningCredentials actual = HashUtility.CreateHmacSha256Signature(signKey);

            // Assert
            // Throw ArgumentException
        }
    }
}