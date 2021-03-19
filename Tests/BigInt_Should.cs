using System;
using System.Numerics;
using FluentAssertions;
using Lab;
using NUnit.Framework;

namespace Tests
{
    class BigInt_Should
    {
        #region cases
        [TestCase("1", "1")]
        [TestCase("0", "1")]
        [TestCase("5486448894889546548966454864488948895465489664", "5486448894889546548966454864488948895465489664")]
        [TestCase("0", "5486448894889546548966454864488948895465489664")]
        #endregion
        public void Check_Equality_Correctly(string sourceA, string sourceB)
        {
            (new BigInt(sourceA) == new BigInt(sourceB)).Should().Be(sourceA.Equals(sourceB));
        }

        #region cases
        [TestCase("-1", "-1")]
        [TestCase("-1", "1")]
        [TestCase("0", "-1")]
        [TestCase("-5486448894889546548966454864488948895465489664", "-5486448894889546548966454864488948895465489664")]
        [TestCase("-5486448894889546548966454864488948895465489664", "5486448894889546548966454864488948895465489664")]
        [TestCase("0", "-5486448894889546548966454864488948895465489664")]
        #endregion
        public void Check_NegativeEquality_Correctly(string sourceA, string sourceB)
        {
            (new BigInt(sourceA) == new BigInt(sourceB)).Should().Be(sourceA.Equals(sourceB));
        }

        #region cases
        [TestCase("100", "105")]
        [TestCase("100", "-105")]
        [TestCase("-100", "105")]
        [TestCase("24562489485236452873", "2456248915612566452873")]
        [TestCase("24562489485236452873", "-2456248915612566452873")]
        [TestCase("-24562489485236452873", "2456248915612566452873")]
        [TestCase("24569489485236452873", "24569489485236452873")]
        [TestCase("24569489485236452873", "-24569489485236452873")]
        [TestCase("-24569489485236452873", "24569489485236452873")]
        #endregion
        public void CompareCorrectly(string sourceA, string sourceB)
        {
            var numbers = GetNumbers(sourceA, sourceB);
            (numbers.a > numbers.b).Should().Be(numbers.x > numbers.y);
            (numbers.a < numbers.b).Should().Be(numbers.x < numbers.y);
        }

        #region cases
        [TestCase("5", "24")]
        [TestCase("5486448664", "2415498298")]
        [TestCase("54864488948895465489664", "241548914984948998298")]
        [TestCase("5486448894889546548966454864488948895465489664", "241548914984948998298241548914984948998298")]
        #endregion
        public void AddCorrectly(string sourceA, string sourceB)
        {
            var (x, y, a, b) = GetNumbers(sourceA, sourceB);
            var correctResult = x + y;
            var actualResult = a + b;
            actualResult.ToString().Should().Be(correctResult.ToString());
        }

        #region cases
        [TestCase("5", "-24")]
        [TestCase("-5", "24")]
        [TestCase("5486448664", "-2415498298")]
        [TestCase("-5486448664", "2415498298")]
        [TestCase("54864488948895465489664", "-241548914984948998298")]
        [TestCase("-54864488948895465489664", "241548914984948998298")]
        [TestCase("5486448894889546548966454864488948895465489664", "-241548914984948998298241548914984948998298")]
        [TestCase("-5486448894889546548966454864488948895465489664", "241548914984948998298241548914984948998298")]
        #endregion
        public void AddCorrectly_WithNegatives(string sourceA, string sourceB)
        {
            var (x, y, a, b) = GetNumbers(sourceA, sourceB);
            var correctResult = x + y;
            var actualResult = a + b;
            actualResult.ToString().Should().Be(correctResult.ToString());
        }

        #region cases
        [TestCase("5", "24")]
        [TestCase("5486448664", "2415498298")]
        [TestCase("54864488948895465489664", "241548914984948998298")]
        [TestCase("5486448894889546548966454864488948895465489664", "241548914984948998298241548914984948998298")]
        #endregion
        public void SubtractCorrectly(string sourceA, string sourceB)
        {
            var numbers = GetNumbers(sourceA, sourceB);
            var correctResult = numbers.x - numbers.y;
            var actualResult = numbers.a - numbers.b;

            actualResult.ToString().Should().Be(correctResult.ToString());
        }

        #region cases
        [TestCase("5", "-24")]
        [TestCase("-5", "24")]
        [TestCase("5486448664", "-2415498298")]
        [TestCase("-5486448664", "2415498298")]
        [TestCase("54864488948895465489664", "-241548914984948998298")]
        [TestCase("-54864488948895465489664", "241548914984948998298")]
        [TestCase("5486448894889546548966454864488948895465489664", "-241548914984948998298241548914984948998298")]
        [TestCase("-5486448894889546548966454864488948895465489664", "241548914984948998298241548914984948998298")]
        #endregion
        public void SubtractCorrectly_WithNegatives(string sourceA, string sourceB)
        {
            var numbers = GetNumbers(sourceA, sourceB);
            var correctResult = numbers.x - numbers.y;
            var actualResult = numbers.a - numbers.b;

            actualResult.ToString().Should().Be(correctResult.ToString());
        }

        [Test]
        public void Throw_OnCreation_WithIncorrectStringValue()
        {
            Assert.Throws(typeof(ArgumentException), () => new BigInt("IncorrectValue"));
            Assert.Throws(typeof(ArgumentException), () => new BigInt("-12634786231784IncorrectValue61782346"));
            Assert.Throws(typeof(ArgumentException), () => new BigInt("IncorrectValue-1263478623178461782346"));
        }

        #region cases
        [TestCase("5", "24")]
        [TestCase("5486448664", "2415498298")]
        [TestCase("54864488948895465489664", "241548914984948998298")]
        [TestCase("5486448894889546548966454864488948895465489664", "241548914984948998298241548914984948998298")]
        #endregion
        public void MultiplyCorrectly(string sourceA, string sourceB)
        {
            var numbers = GetNumbers(sourceA, sourceB);
            var correctResult = numbers.x * numbers.y;
            var actualResult = numbers.a * numbers.b;
            actualResult.ToString().Should().Be(correctResult.ToString());
        }

        #region cases
        [TestCase("5", "-24")]
        [TestCase("-5", "24")]
        [TestCase("5486448664", "-2415498298")]
        [TestCase("-5486448664", "2415498298")]
        [TestCase("54864488948895465489664", "-241548914984948998298")]
        [TestCase("-54864488948895465489664", "241548914984948998298")]
        [TestCase("5486448894889546548966454864488948895465489664", "-241548914984948998298241548914984948998298")]
        [TestCase("-5486448894889546548966454864488948895465489664", "241548914984948998298241548914984948998298")]
        [TestCase("7326578284562154865218376525147361731", "90865747564345764387235472632345324671")]
        #endregion
        public void MultiplyCorrectly_WithNegatives(string sourceA, string sourceB)
        {
            var numbers = GetNumbers(sourceA, sourceB);
            var correctResult = numbers.x * numbers.y;
            var actualResult = numbers.a * numbers.b;
            actualResult.ToString().Should().Be(correctResult.ToString());
        }

        #region cases
        [TestCase("5", "24")]
        [TestCase("5486448664", "2415498298")]
        [TestCase("54864488948895465489664", "241548914984948998298")]
        [TestCase("5486448894889546548966454864488948895465489664", "241548914984948998298241548914984948998298")]
        #endregion
        public void DivideCorrectly(string sourceA, string sourceB)
        {
            var numbers = GetNumbers(sourceA, sourceB);
            var correctResult = numbers.x / numbers.y;
            var actualResult = numbers.a / numbers.b;
            actualResult.ToString().Should().Be(correctResult.ToString());
        }

        #region cases
        [TestCase("5", "-24")]
        [TestCase("-5", "24")]
        [TestCase("5486448664", "-2415498298")]
        [TestCase("-5486448664", "2415498298")]
        [TestCase("54864488948895465489664", "-241548914984948998298")]
        [TestCase("-54864488948895465489664", "241548914984948998298")]
        [TestCase("5486448894889546548966454864488948895465489664", "-241548914984948998298241548914984948998298")]
        [TestCase("-5486448894889546548966454864488948895465489664", "241548914984948998298241548914984948998298")]
        [TestCase("6657350128023873755299597", "3")]
        #endregion
        public void DivideCorrectly_WithNegatives(string sourceA, string sourceB)
        {
            var numbers = GetNumbers(sourceA, sourceB);
            var correctResult = numbers.x / numbers.y;
            var actualResult = numbers.a / numbers.b;
            actualResult.ToString().Should().Be(correctResult.ToString());
        }

        #region cases
        [TestCase("5", "24")]
        [TestCase("5486448664", "2415498298")]
        [TestCase("54864488948895465489664", "241548914984948998298")]
        [TestCase("1849", "91")]
        #endregion
        public void FindRemainderCorrectly(string sourceA, string sourceB)
        {
            var numbers = GetNumbers(sourceA, sourceB);
            var correctResult = numbers.x % numbers.y;
            var actualResult = numbers.a % numbers.b;
            actualResult.ToString().Should().Be(correctResult.ToString());
        }

        #region cases
        [TestCase("5", "-24")]
        [TestCase("-5", "24")]
        [TestCase("5486448664", "-2415498298")]
        [TestCase("-5486448664", "2415498298")]
        [TestCase("54864488948895465489664", "-241548914984948998298")]
        [TestCase("-54864488948895465489664", "241548914984948998298")]
        [TestCase("5486448894889546548966454864488948895465489664", "-241548914984948998298241548914984948998298")]
        [TestCase("-5486448894889546548966454864488948895465489664", "241548914984948998298241548914984948998298")]
        [TestCase("6657350128023873755299597", "3")]
        #endregion
        public void FindRemainderCorrectly_WithNegatives(string sourceA, string sourceB)
        {
            var numbers = GetNumbers(sourceA, sourceB);
            var correctResult = numbers.x % numbers.y;
            var actualResult = numbers.a % numbers.b;
            actualResult.ToString().Should().Be(correctResult.ToString());
        }

        #region cases
        [TestCase("123456789", "987654321")]
        [TestCase("-123456789", "987654321")]
        [TestCase("-123456789", "-987654321")]
        [TestCase("92345675645648948948944896489", "987494982498248949198419498491654321")]
        [TestCase("9234567564564894894646527843562138974572514987253146913487538764823581364873261874623179059832165807132604758944896489", 
            "98234750943729056394208587012874632874687346587432659784236756349825987328958742659824875682498768254238756298649236784")]
        #endregion
        public void FindGcdCorrectly(string firstNumber, string secondNumber)
        {
            var (x, y, a, b) = GetNumbers(firstNumber, secondNumber);
            BigInt.GreatestCommonDivisor(a, b, out _, out _).ToString()
                .Should().Be(BigInteger.GreatestCommonDivisor(x, y).ToString());
        }

        #region cases
        [TestCase("1896514984", "814989874", "891461468")]
        [TestCase("-1896514984", "814989874", "891461468")]
        [TestCase("18965149634756842376572364897564238958723468762075674327859872649024378567843257645876581586458731598784608751278587184", 
            "81239847327164872135463218549817236492315874632819548721534651236547862315489723169845238916598514989874", 
            "81234876321874623916587236159812380947892365823149874759084635897614858217648123687462837691461468")]
        #endregion
        public void FindPowerByModuleCorrectly(string value, string power, string module)
        {
            var val = new BigInt(value);
            var pow = new BigInt(power);
            var mod = new BigInt(module);
            var val2 = BigInteger.Parse(value);
            var pow2 = BigInteger.Parse(power);
            var mod2 = BigInteger.Parse(module);

            BigInt.ModulePower(val, pow, mod).ToString()
                .Should().Be(BigInteger.ModPow(val2, pow2, mod2).ToString());
        }

        #region cases
        [TestCase(100)]
        [TestCase(500)]
        [TestCase(1000)]
        [TestCase(5000)]
        [TestCase(10000)]
        #endregion
        public void WorkWithVeryBigNumbers(int digitsAmount)
        {
            var valBase = new string('9', digitsAmount);
            var value = new BigInt(valBase);
            var value2 = BigInteger.Parse(valBase);

            (value + value).ToString().Should().Be((value2 + value2).ToString());
            (value - value).ToString().Should().Be((value2 - value2).ToString());
            (value * value).ToString().Should().Be((value2 * value2).ToString());
            ((value * new BigInt(2)) / value).ToString().Should().Be(((value2 * 2) / value2).ToString());

            (value == value + BigInt.One).Should().BeFalse();
            (value < value + BigInt.One).Should().BeTrue();
            (value > value + BigInt.One).Should().BeFalse();

            (value % (value + BigInt.One)).ToString().Should().Be((value2 % (value2 + 1)).ToString());
            (value % (value + BigInt.One)).ToString().Should().Be((value2 % (value2 + 1)).ToString());

            BigInt.GreatestCommonDivisor(value, value / new BigInt(2), out _, out _).ToString()
                .Should().Be(BigInteger.GreatestCommonDivisor(value2, value2 / 2).ToString());
        }

        public (BigInteger x, BigInteger y, BigInt a, BigInt b) GetNumbers(string sourceA, string sourceB)
        {
            var x = BigInteger.Parse(sourceA);
            var y = BigInteger.Parse(sourceB);
            var a = new BigInt(sourceA);
            var b = new BigInt(sourceB);

            return (x, y, a, b);
        }
    }
}
