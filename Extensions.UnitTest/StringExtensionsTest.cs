using Extensions.UnitTest.OtherClasses;
using System;
using System.Linq;
using Xunit;

namespace Extensions.UnitTest
{
    public class StringExtensionsTest
    {
        [Theory]
        [InlineData("09/10/2019", "dd/MM/yyyy", true)]
        [InlineData("09/13/2019", "dd/MM/yyyy", false)]
        [InlineData("11/28/2019", "MM/dd/yyyy", true)]
        public void IsDateTime_OnlyDateInddMMyyyy(string actual, string format, bool res)
        {
            bool isEqual = actual.IsDateTime(format);
            Assert.True(res == isEqual);
        }

        [Theory]
        [InlineData(12, "Swagat Swain", 25, ",", "12,Swagat Swain,25")]
        [InlineData(40, "", 25, "|", "40||25")]
        [InlineData(40, null, 25, "|", "40||25")]
        public void ToCSVString_TestUser(int id, string name, int age, string separator, string result)
        {
            var user = new TestUser { TestUserId = id, Name = name, Age = age };
            Assert.Equal(result, user.ToCSVString(separator));
        }

        [Fact]
        public void SplitTo_WithoutStringOptions_IntegerShouldPass()
        {
            var actual = new int[] { 23, 34, 56, -2, 33, 100 };
            var bar = "23,34,56,-2,33,100";
            var expected = bar.SplitTo<int>(',').ToArray();
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void SplitTo_WithoutStringOptions_DecimalShouldPass()
        {
            var actual = new decimal[] { 23.6M, 34.6M, 56, -2, 33, 100 };
            var bar = "23.6,34.6,56,-2,33,100";
            var expected = bar.SplitTo<decimal>(',').ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SplitTo_WithStringOptions_IntegerShouldPass()
        {
            var actual = new int[] { 23, 34, 56, -2, 33, 100 };
            var bar = "23,34,,56,,-2,33,100";
            var expected = bar.SplitTo<int>(StringSplitOptions.RemoveEmptyEntries, ',').ToArray();
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void SplitTo_WithStringOptions_DecimalShouldPass()
        {
            var actual = new decimal[] { 23.6M, 34.6M, 56, -2, 33, 100 };
            var bar = "23.6,,34.6,56,-2,33,,100";
            var expected = bar.SplitTo<decimal>(StringSplitOptions.RemoveEmptyEntries, ',').ToArray();
            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData("true", true)]
        [InlineData("t", true)]
        [InlineData("t ", true)]
        [InlineData("True ", true)]
        [InlineData("TRUE ", true)]
        [InlineData("yes", true)]
        [InlineData("false", false)]
        [InlineData("f ", false)]
        [InlineData("no", false)]
        [InlineData("False", false)]
        [InlineData("FALSE", false)]
        public void ToBoolean_ShouldPass(string actual, bool expected)
        {
            Assert.True(actual.ToBoolean() == expected);
        }
        [Theory]
        [InlineData("")]
        [InlineData("tR")]
        [InlineData("  ")]
        public void ToBoolean_Exception_ArgumentException(string actual)
        {
            Assert.Throws<ArgumentException>(() => actual.ToBoolean());
        }

        [Theory]
        [InlineData("Admin", 1)]
        [InlineData("PublicUser", 2)]
        [InlineData("Supervisor", 3)]
        [InlineData(" ", 0)]
        public void ToEnum_ShouldPass(string data, int expected)
        {
            var actual = Convert.ToInt32(data.ToEnum<TestEnums>());
            Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData(" ", "")]
        [InlineData("", "")]
        [InlineData("Swagat ", "Swagat")]
        public void GetEmptyStringIfNull_ShouldPass(string data, string expected)
        {
            Assert.True(data.GetEmptyStringIfNull() == expected);
        }
        [Theory]
        [InlineData(null, null)]
        [InlineData(" ", null)]
        [InlineData("", null)]
        [InlineData(" Swagat ", "Swagat")]
        public void GetNullIfEmptyString_ShouldPass(string data, string expected)
        {
            Assert.True(data.GetNullIfEmptyString() == expected);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData(" ", false)]
        [InlineData("", false)]
        [InlineData("12 ", true)]
        [InlineData(" 112 ", true)]
        [InlineData("-112 ", true)]
        public void IsInteger_ShouldPass(string data, bool expected)
        {
            Assert.True(data.IsInteger() == expected);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData(" ", false)]
        [InlineData("", false)]
        [InlineData("12 ", true)]
        [InlineData(" 112 ", true)]
        [InlineData("-112 ", true)]
        [InlineData(" -112.0 ", true)]
        [InlineData("-112.9 ", true)]
        [InlineData("123.09000 ", true)]
        public void IsDecimal_ShouldPass(string data, bool expected)
        {
            Assert.True(data.IsDecimal() == expected);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(" Hello World", " ")]
        [InlineData("", null)]
        [InlineData("12 ", "1")]
        [InlineData(" 112 ", " ")]
        [InlineData("-112", "-")]
        [InlineData("Royal College", "R")]
        public void FirstCharacter_ShouldPass(string data, string expected)
        {
            Assert.Equal(data.FirstCharacter(), expected);
        }
        [Theory]
        [InlineData(null, null)]
        [InlineData(" Hello World", "d")]
        [InlineData("", null)]
        [InlineData("12 ", " ")]
        [InlineData(" 112 ", " ")]
        [InlineData("-112", "2")]
        [InlineData("Royal College", "e")]
        public void LastCharacter_ShouldPass(string data, string expected)
        {
            Assert.Equal(data.LastCharacter(), expected);
        }

        [Theory]
        [InlineData("Hello World", "rld", true)]
        [InlineData("Hello World ", "rld", false)]
        [InlineData("Hello World", "Orld", true)]
        [InlineData("Hello World", "World", true)]
        [InlineData("Hello World", "Hello World", true)]
        [InlineData("Hello World", "Swagat Hello World", false)]
        public void EndsWithIgnoreCase_ShouldPass(string data, string endsWith, bool expected)
        {
            Assert.True(data.EndsWithIgnoreCase(endsWith) == expected);
        }
        [Theory]
        [InlineData(null, "World", "val")]
        [InlineData("World", null, "suffix")]
        public void EndsWithIgnoreCase_ArgumentNullException_ShouldFail(string data, string suffix, string param)
        {
            Assert.Throws<ArgumentNullException>(param, () => data.EndsWithIgnoreCase(suffix));
        }

        [Theory]
        [InlineData("Hello World", "hell", true)]
        [InlineData("Hello World ", " hel", false)]
        [InlineData("Hello World", "HelLo", true)]
        [InlineData("Hello World", "Hello", true)]
        [InlineData("Hello World", "Hello World", true)]
        [InlineData("Hello World", "Swagat Hello World", false)]
        [InlineData(" Hello World", "Hello World", false)]
        [InlineData(" Hello World", " Hello World", true)]
        public void StartsWithIgnoreCase_ShouldPass(string data, string endsWith, bool expected)
        {
            Assert.True(data.StartsWithIgnoreCase(endsWith) == expected);
        }
        [Theory]
        [InlineData(null, "World", "val")]
        [InlineData("World", null, "prefix")]
        public void StartsWithIgnoreCase_ArgumentNullException_ShouldFail(string data, string prefix, string param)
        {
            Assert.Throws<ArgumentNullException>(param, () => data.StartsWithIgnoreCase(prefix));
        }

        [Theory]
        [InlineData("swagat", "Swagat")]
        [InlineData(" swagat", " swagat")]
        [InlineData("swagat kumar swain", "Swagat kumar swain")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void Capitalize_ShouldPass(string data, string actual)
        {
            Assert.Equal(data.Capitalize(), actual);
        }

        [Theory]
        [InlineData("swagat swain", "s,w,a,i,n", "gt ")]
        [InlineData("swagat swain ", " ", "swagatswain")]
        public void RemoveChars_ShouldPass(string data, string replaceString, string actual)
        {
            var res = data.RemoveChars(replaceString.ToCharArray());
            Assert.Equal(res, actual);
        }
        [Theory]
        [InlineData("swagat swain", "S,w,a,i,n", "gt ")]
        [InlineData("swagat swain ", " ", "swagatswain")]
        public void RemoveCharsIgnoreCase_ShouldPass(string data, string replaceString, string actual)
        {
            var res = data.RemoveCharsIgnoreCase(replaceString.ToCharArray());
            Assert.Equal(res, actual);
        }
        [Theory]
        [InlineData("swagat swain", "swain", "swagat ")]
        [InlineData("swagat swain", "ain", "swagat sw")]
        [InlineData("swagat swain", "Swagat", "swagat swain")]
        [InlineData("swagat swain is swagat swain", "swagat", " swain is  swain")]
        [InlineData("swagat swain is swagat swain", "is swagat swain", "swagat swain ")]
        [InlineData("swagat swain is swagat swain", "agat swain", "sw is sw")]
        public void RemoveString_ShouldPass(string data, string replaceString, string actual)
        {
            var res = data.RemoveString(replaceString);
            Assert.Equal(res, actual);
        }

        [Theory]
        [InlineData("swagat swain", "swain", "swagat ")]
        [InlineData("swagat swain", "SWAIN", "swagat ")]
        [InlineData("swagat swain", "ain", "swagat sw")]
        [InlineData("swagat swain", "Swagat", " swain")]
        [InlineData("swagat swain is swagat swain", "swagat", " swain is  swain")]
        [InlineData("swagat swain is swagat swain", "is SWagat swain", "swagat swain ")]
        [InlineData("swagat swain is swagat swain", "agat swain", "sw is sw")]
        public void RemoveStringIgnoreCase_ShouldPass(string data, string replaceString, string actual)
        {
            var res = data.RemoveStringIgnoreCase(replaceString);
            Assert.Equal(res, actual);
        }
    }
}