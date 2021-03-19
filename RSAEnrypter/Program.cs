using System;
using System.IO;
using Lab;

namespace RSAEnrypter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter command: ");
            var input = Console.ReadLine();
            while ((!string.IsNullOrEmpty(input)))
            {
                var parsed = input.Trim().Split(' ');
                switch (parsed[0])
                {
                    case "encrypt":
                        EncryptString();
                        break;
                    case "decrypt":
                        DecryptString();
                        break;
                    case "encryptf":
                        if (parsed.Length > 1)
                            EncryptFile(parsed[1]);
                        else
                            Console.WriteLine("Invalid file path.");
                        break;
                    case "decryptf":
                        if (parsed.Length > 1)
                            DecryptFile(parsed[1]);
                        else
                            Console.WriteLine("Invalid file path.");
                        break;
                    case "help":
                        Console.WriteLine("encrypt\t\t:Encrypts string with RSA\n" +
                                          "decrypt\t\t:Decrypts encrypted string with RSA key\n" +
                                          "encryptf {path}\t\t:Encrypts file with RSA\n" +
                                          "decryptf {path}\t\t:Decrypts file with RSA");
                        break;
                    default:
                        Console.WriteLine("Invalid command. Type \"help\" to see availible commands");
                        break;
                }

                Console.WriteLine("Enter command: ");
                input = Console.ReadLine();
            }
        }

        private static void EncryptString()
        {
            Console.WriteLine("Enter first prime number: ");
            var firstPrime = Console.ReadLine();
            Console.WriteLine("Enter second prime number: ");
            var secondPrime = Console.ReadLine();
            Console.WriteLine("Enter your string to encrypt: ");

            var (encryptedValue, publicKey, secretKey) =
                RSAEncrypter.EncryptString(firstPrime, secondPrime, Console.ReadLine());

            Console.WriteLine("Encrypted data:");
            Console.WriteLine(encryptedValue);
            Console.WriteLine($"Your secret key: (exponent: {secretKey.Exponent}; module: {secretKey.Module})");
            Console.WriteLine($"Your public key: (exponent: {publicKey.Exponent}; module: {publicKey.Module})");
        }

        private static void DecryptString()
        {
            Console.WriteLine("Enter your secret key's exponent: ");
            var exp = new BigInt(Console.ReadLine());
            Console.WriteLine("Enter your key's module: ");
            var module = new BigInt(Console.ReadLine());
            Console.WriteLine("Enter your encrypted data: ");
            var encrypted = Console.ReadLine();

            Console.WriteLine($"Your string is: \n{RSAEncrypter.DecryptString(exp, module, encrypted)}");
        }

        private static void EncryptFile(string path)
        {
            if(!File.Exists(path))
                throw new FileNotFoundException("File does not exist.");

            Console.WriteLine("Enter first prime number: ");
            var firstPrime = Console.ReadLine();
            Console.WriteLine("Enter second prime number: ");
            var secondPrime = Console.ReadLine();
            var (newPath, publicKey, secretKey) = RSAEncrypter.EncryptFile(path, firstPrime, secondPrime);

            Console.WriteLine($"Encrypted file saved by path: {newPath}");
            Console.WriteLine($"Your secret key: (exponent: {secretKey.Exponent}; module: {secretKey.Module})");
            Console.WriteLine($"Your public key: (exponent: {publicKey.Exponent}; module: {publicKey.Module})");
        }

        private static void DecryptFile(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("File does not exist.");

            Console.WriteLine("Enter your key's exponent: ");
            var exp = new BigInt(Console.ReadLine());
            Console.WriteLine("Enter your key's module: ");
            var module = new BigInt(Console.ReadLine());

            Console.WriteLine($"Decrypted file saved by path: {RSAEncrypter.DecryptFile(path, exp, module)}");
        }
    }
}
