using Lab;

namespace RSAEnrypter
{
    public static class RSAKeysGenerator
    {
        public static (Key publicKey, Key secretKey) GetKeys(string firstPrime, string secondPrime)
        {
            var p = new BigInt(firstPrime);
            var q = new BigInt(secondPrime);
            var module = p * q;
            var eulerValue = GetEulerFunctionValue(p, q);
            var publicExponent = GetPublicExponent(eulerValue);
            var secretExponent = GetSecretExponent(publicExponent, eulerValue);

            var publicKey = new Key(publicExponent, module);
            var secretKey = new Key(secretExponent, module);

            return (publicKey, secretKey);
        }

        private static BigInt GetEulerFunctionValue(BigInt p, BigInt q) => (p - BigInt.One) * (q - BigInt.One);

        private static BigInt GetSecretExponent(BigInt exp, BigInt eulerValue) =>
            BigInt.GetModuleInversed(exp, eulerValue);

        private static BigInt GetPublicExponent(BigInt module)
        {
            var exp = new BigInt(3);

            for (var i = BigInt.Zero; i < module; i++)
            {
                if (BigInt.GreatestCommonDivisor(exp, module, out _, out _) == BigInt.One)
                    return exp;
                exp += BigInt.One;
            }

            return exp;
        }
    }
}