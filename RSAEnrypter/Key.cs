using Lab;

namespace RSAEnrypter
{
    public class Key
    {
        public BigInt Exponent { get; }
        public BigInt Module { get; }

        public Key(BigInt exponent, BigInt module)
        {
            Exponent = exponent;
            Module = module;
        }
    }
}
