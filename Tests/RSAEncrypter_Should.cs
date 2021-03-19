using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Lab;
using NUnit.Framework;
using RSAEnrypter;

namespace Tests
{
    class RSAEncrypter_Should
    {
        #region cases
        [TestCase("13","17", "test string")]
        [TestCase("3557","2579", "test string")]
        [TestCase("91419849824949498149483", "81649814542641687673", "test string")]
        [TestCase("5037569", "81649814542641687673", "test string")]
        [TestCase("91419849824949498149483", "5810011", "test string")]
        [TestCase("103", "5810011", "test string")]
        #endregion
        public void WorkCorrectly_WithString(string firstPrime, string secondPrime, string value)
        {
            var encryptionResult = RSAEncrypter.EncryptString(firstPrime, secondPrime, value);
            encryptionResult.encryptedValue.Should().NotBe(value);

            RSAEncrypter.DecryptString(encryptionResult.secretKey.Exponent, encryptionResult.secretKey.Module,
                encryptionResult.encryptedValue).Should().Be(value);
        }

        #region cases
        [TestCase("13", "17")]
        [TestCase("3557", "2579")]
        [TestCase("480463", "718943")]
        #endregion
        public void WorkCorrectly_WithFile(string firstPrime, string secondPrime)
        {
            var defaultPath = Environment.CurrentDirectory + "test.txt";
            File.WriteAllLines(defaultPath, GenerateFileContent(250, 100));
            var encryptionResult =
                RSAEncrypter.EncryptFile(defaultPath, firstPrime, secondPrime);

            File.ReadLines(encryptionResult.newPath).Should().NotEqual(File.ReadLines(defaultPath));

            var decryptionResult = RSAEncrypter.DecryptFile(encryptionResult.newPath,
                encryptionResult.secretKey.Exponent, encryptionResult.secretKey.Module);

            File.ReadLines(decryptionResult).Should().Equal(File.ReadLines(defaultPath));

            File.Delete(defaultPath);
            File.Delete(encryptionResult.newPath);
            File.Delete(decryptionResult);
        }

        private static IEnumerable<string> GenerateFileContent(int stringLength, int stringsAmount)
        {
            var str = new string('a', stringLength);

            for (var i = 0; i < stringsAmount; i++)
                yield return str;
        }
    }
}
