using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Lab;

namespace RSAEnrypter
{
    public static class RSAEncrypter
    {
        public static (string encryptedValue, Key publicKey, Key secretKey) EncryptString(string firstPrime, string secondPrime, string str)
        {
            var (publicKey, secretKey) = RSAKeysGenerator.GetKeys(firstPrime, secondPrime);
            var data = Encoding.ASCII.GetBytes(str ?? string.Empty).Select(x => (int) x).ToArray();
            var encrypted = Encrypt(data, publicKey.Exponent, publicKey.Module);
            
            return (string.Join(":", encrypted), publicKey, secretKey);
        }

        public static string DecryptString(BigInt exp, BigInt module, string encrypted)
        {
            var data = encrypted.Split(':').Select(x => new BigInt(x));
            var decryptedData = Decrypt(data, exp, module);

            return Encoding.ASCII.GetString(decryptedData.Select(x => (byte) x).ToArray());
        }

        public static (string newPath, Key publicKey, Key secretKey) EncryptFile(string path, string firstPrime, string secondPrime)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("File does not exist.");

            var file = File.ReadLines(path);
            var (publicKey, secretKey) = RSAKeysGenerator.GetKeys(firstPrime, secondPrime);
            var encrypted =
                file.Select(x => Encoding.ASCII.GetBytes(x).Select(b => (int) b))
                    .Select(x => string.Join(":", Encrypt(x, publicKey.Exponent, publicKey.Module)));
            File.WriteAllLines(path + ".enc", encrypted);
            return (path + ".enc", publicKey, secretKey);
        }

        public static string DecryptFile(string path, BigInt exp, BigInt module)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("File does not exist.");

            var file = File.ReadLines(path);
            var decrypted = 
                file.Select(x => x.Split(':').Select(n => new BigInt(n)))
                    .Select(x => Decrypt(x, exp, module).Select(n => (byte)n))
                    .Select(x => Encoding.ASCII.GetString(x.ToArray()));
            File.WriteAllLines(path + ".txt", decrypted);

            return path + ".txt";
        }

        private static IEnumerable<BigInt> Encrypt(IEnumerable<int> bytes, BigInt e, BigInt module)
        {
            return bytes.Select(b => BigInt.ModulePower(new BigInt(b.ToString()), e, module)).ToList();
        }

        private static IEnumerable<int> Decrypt(IEnumerable<BigInt> encoded, BigInt d, BigInt module)
        {
            return encoded.Select(value => BigInt.ModulePower(value, d, module))
                .Select(code => int.Parse(code.ToString())).ToArray();
        }
    }
}