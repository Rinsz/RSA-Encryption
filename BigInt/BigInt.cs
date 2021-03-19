using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lab
{
    public class BigInt
    {
        private int sign = 0;

        public int Sign => Length > 0 ? sign : 0;

        private List<byte> digits = new List<byte>();

        private protected int _length;

        private protected int Length
        {
            get => _length;
            set
            {
                _length = value;
                if (value == 0)
                    sign = 0;
            }
        }

        public static BigInt Zero => new BigInt();

        public static BigInt One => new BigInt(1);

        public BigInt() => sign = 0;

        public BigInt(int value) => SetFromNumber(value);

        public BigInt(uint value) => SetFromUnsigned(value);

        public BigInt(long value) => SetFromNumber(value);

        public BigInt(ulong value) => SetFromUnsigned(value);

        public BigInt(string value)
        {
            if (value is null)
                throw new ArgumentException("It's impossible to create value from null");

            if (!Regex.IsMatch(value, "^[-1234567890]\\d*$"))
                throw new ArgumentException("It's impossible to create value from invalid string");

            sign = value[0] == '-' ? -1 : 1;
            digits = value.SkipWhile(x => x == '0' || !char.IsDigit(x)).Select(x => (byte) char.GetNumericValue(x)).Reverse().ToList();
            Length = digits.Count;
        }

        public BigInt(char sign, IEnumerable<byte> value)
        {
            this.sign = sign == '-' ? -1 : 1;
            digits = value.SkipWhile(x => x == 0).Reverse().ToList();
            Length = digits.Count;
        }

        public BigInt(BigInt value)
        {
            sign = value.sign;
            digits = value.digits.FindAll(_ => true);
            Length = value.Length;
        }

        private void SetFromNumber(long value)
        {
            sign = value > 0 ? 1 : -1;
            digits = SplitToBytes((ulong)(value * sign));
            Length = digits.Count;
        }

        private void SetFromUnsigned(ulong value)
        {
            sign = 1;
            digits = SplitToBytes(value);
            Length = digits.Count;
        }

        private static List<byte> SplitToBytes(ulong num)
        {
            var numDigits = new List<byte>();
            if (num == 0)
                return numDigits;
            do
            {
                numDigits.Add((byte) (num % 10));
                num /= 10;
            } while (num > 0);

            return numDigits;
        }

        private static BigInt Add(int sign, BigInt left, BigInt right)
        {
            var result = new List<byte> {0};

            if (left.Length < right.Length)
            {
                var t = left;
                left = right;
                right = t;
            }

            for (var i = 0; i < left.Length; i++)
            {
                var sum = i < right.Length
                    ? left.digits[i] + right.digits[i] + result[i]
                    : left.digits[i] + result[i];
                result[i] = (byte) (sum % 10);
                result.Add((byte) (sum / 10));
            }

            return new BigInt(sign < 0 ? '-' : '+', Enumerable.Reverse(result).ToList());
        }

        private static BigInt Subtract(char sign, BigInt left, BigInt right)
        {
            if (left.Abs() < right.Abs())
            {
                var temp = left;
                left = right;
                right = temp;
            }

            var result = new List<int>(left.digits.Select(x => (int)x));

            for (var i = 0; i < right.Length; i++)
            {
                if (result[i] < right.digits[i])
                {
                    result[i] = (byte)(result[i] + 10 - right.digits[i]);
                    result[i + 1]--;
                }
                else
                    result[i] -= right.digits[i];
            }

            return new BigInt(sign, result.Select(x => (byte)x).Reverse());
        }

        private static BigInt MultiplyOnDigit(BigInt source, int multiplier)
        {
            if (multiplier > 9 || multiplier < 0)
                throw new ArgumentException();

            if (multiplier == 0)
                return Zero;

            var result = new List<byte> {0};
            for (var i = 0; i < source.digits.Count; i++)
            {
                var multiplicationResult = source.digits[i] * multiplier;
                result[i] += (byte) (multiplicationResult % 10);
                if (result[i] >= 10)
                {
                    result.Add((byte) (result[i] / 10 + multiplicationResult / 10));
                    result[i] %= 10;
                }
                else
                    result.Add((byte) (multiplicationResult / 10));
            }

            return new BigInt(source.sign > 0 ? '+' : '-', Enumerable.Reverse(result).SkipWhile(x => x == 0).ToList());
        }

        private static BigInt MultiplyOnTen(BigInt source, int powerOfTen = 1)
        {
            if (powerOfTen < 0)
                throw new ArgumentException();

            if (powerOfTen == 0)
                return source;

            var result = Enumerable.Reverse(source.digits).Concat(Enumerable.Repeat((byte) 0, powerOfTen));
            return new BigInt(source.sign > 0 ? '+' : '-', result.ToList());
        }

        private static BigInt Divide(BigInt dividend, BigInt divisor)
        {
            if (dividend.Abs() < divisor.Abs())
                return Zero;

            if (dividend == divisor)
                return One;

            var dividendDigits = Enumerable.Reverse(dividend.digits).ToList();
            var result = new List<byte>();

            var tens = 0;
            for (var i = 0; dividend.digits[i] == 0 && divisor.digits[i] == 0; i++)
                tens++;

            var index = divisor.Length - 1;
            var minuend = new BigInt('+', dividendDigits.Take(divisor.Length).ToList());
            var subtrahend = divisor.Abs();
            while (index < dividend.Length)
            {
                var flag = false;
                while (minuend < subtrahend && index + 1 < dividend.Length)
                {
                    minuend = new BigInt('+', Enumerable.Reverse(minuend.digits).Append(dividendDigits[++index]));
                    if (flag)
                        result.Add(0);
                    flag = true;
                }

                if (minuend < subtrahend && index + 1 == dividend.Length)
                {
                    if (flag)
                        result.Add(0);
                    break;
                }

                var multiplier = 1;
                while (!(subtrahend * new BigInt(multiplier + 1) > minuend))
                    multiplier++;
                
                result.Add((byte)multiplier);
                minuend -= MultiplyOnDigit(subtrahend, multiplier);
            }

            return new BigInt('+', result.Concat(Enumerable.Repeat((byte) 0, tens)));
        }

        public static BigInt GreatestCommonDivisor(BigInt number, BigInt secondNumber, out BigInt x, out BigInt y)
        {
            if (number == Zero)
            {
                x = Zero;
                y = One;
                return secondNumber;
            }

            var divisor = GreatestCommonDivisor(secondNumber % number, number, out var x1, out var y1);
            x = y1 - (secondNumber / number) * x1;
            y = x1;
            return divisor.Abs();
        }

        public static BigInt GetModuleInversed(BigInt left, BigInt right)
        {
            var divisor = GreatestCommonDivisor(left, right, out var x, out _);

            if (divisor != One)
                return Zero;

            return (x % right + right) % right;
        }

        public static BigInt ModulePower(BigInt value, BigInt power, BigInt module)
        {
            var binaryPower = ToBinary(power);
            var result = One;
            foreach (var charge in binaryPower)
            {
                if (charge == One)
                    result = (result * value) % module;
                value = (value * value) % module;
            }
            return result;
        }

        private static List<BigInt> ToBinary(BigInt value)
        {
            var clone = new BigInt(value);
            var bin = new BigInt(2);
            var result = new List<BigInt>();
            while (clone != Zero)
            {
                result.Add(clone % bin);
                clone /= bin;
            }

            return result;
        }

        public BigInt Abs() => new BigInt('+', Enumerable.Reverse(digits).ToList());

        public override string ToString() => Length != 0
            ? $"{(sign == -1 ? "-" : "")}{string.Join(null, Enumerable.Reverse(digits))}"
            : "0";

        public static BigInt operator +(BigInt first, BigInt second)
        {
            if (first.Length == 0)
                return second;
            if (second.Length == 0)
                return first;

            if (first.sign == second.sign) 
                return Add(first.sign, first, second);

            return first.sign < 0 ? second - -first : first - -second;
        }

        public static BigInt operator ++(BigInt val) => val + One;

        public static BigInt operator --(BigInt val) => val - One;

        public static BigInt operator -(BigInt source)
        {
            return new BigInt(source.sign * -1 < 0 ? '-' : '+', Enumerable.Reverse(source.digits));
        }

        public static BigInt operator -(BigInt first, BigInt second)
        {
            if (first.Length == 0)
                return -second;
            if (second.Length == 0)
                return first;

            if (first.sign != second.sign)
                return first.sign < 0 ? -(first.Abs() + second.Abs()) : first.Abs() + second.Abs();

            if (first.sign > 0 && second > first || first.sign < 0 && first > second)
                return Subtract('-', first, second);

            return Subtract('+', first, second);
        }

        public static BigInt operator *(BigInt first, BigInt second)
        {
            if (first.Length < second.Length)
            {
                var temp = first;
                first = second;
                second = temp;
            }

            var sum = new BigInt();

            for (var i = 0; i < second.digits.Count; i++)
            {
                sum += MultiplyOnTen(MultiplyOnDigit(first, second.digits[i]), i);
            }

            return second.Sign < 0 ? -sum : sum;
        }

        public static BigInt operator /(BigInt first, BigInt second)
        {
            var result = Divide(first, second);
            return first.Sign == second.Sign ? result : -result;
        }

        public static BigInt operator %(BigInt first, BigInt second) => first - second * (first / second);
        
        public static bool operator == (BigInt first, BigInt second)
        {
            if (first is null || second is null)
                throw new InvalidOperationException("It's impossible to compare with null");
            if (first.sign != second.sign)
                return false;
            if (first.Length != second.Length)
                return false;
            for (var i = 0; i < first.Length; i++)
                if (first.digits[i] != second.digits[i])
                    return false;
            return true;
        }

        public static bool operator !=(BigInt first, BigInt second) => !(first == second);

        public static bool operator >(BigInt first, BigInt second)
        {
            if (first is null || second is null)
                throw new InvalidOperationException("It's impossible to compare with null");
            
            if (first.sign != second.sign)
                if (first.Sign == 0 && second.Sign < 0 || first.Sign > 0 && second.Sign == 0)
                    return true;
                else 
                    return second.sign < 0;

            if (first.Length != second.Length)
                return first.Length > second.Length;

            for (var i = first.Length - 1; i >= 0 ; i--)
                if (first.digits[i] != second.digits[i])
                    return first.digits[i] > second.digits[i];
            return false;
        }

        public static bool operator <(BigInt first, BigInt second) => second > first;
    }
}
