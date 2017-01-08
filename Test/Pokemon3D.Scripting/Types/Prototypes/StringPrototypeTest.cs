using NUnit.Framework;
using Pokemon3D.Scripting.Types;

namespace Test.Pokemon3D.Scripting.Types.Prototypes
{
    [TestFixture]
    public class StringPrototypeTest
    {
        [TestCase("Hello World")]
        [TestCase("")]
        public void StringLengthIsCorrect(string value)
        {
            var result = ScriptProcessorFactory.Run($"var str = new String('{value}'); str.length;");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(value.Length));
        }

        [Test]
        public void EmptyStringWorks()
        {
            var result = ScriptProcessorFactory.Run("var str = String.empty; str;");

            Assert.That(result, Is.InstanceOf<SString>());
            Assert.That(((SString)result).Value, Is.EqualTo(""));
        }

        [TestCase("str.remove(6);", "Hello ")]
        [TestCase("str.remove(20);", "Hello World")]
        [TestCase("str.remove(-1);", "")]
        [TestCase("str.remove(0, 4);", "o World")]
        [TestCase("str.remove(0, 20);", "")]
        [TestCase("str.remove(0, -1);", "Hello World")]
        public void RemoveWorks(string script, string expectedResult)
        {
            var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); " + script);

            Assert.That(result, Is.InstanceOf<SString>());
            Assert.That(((SString)result).Value, Is.EqualTo(expectedResult));
        }

        [TestCase("str.trim();", " Hello World ", "Hello World")]
        [TestCase("str.trim(['{', '}']);", "{Hello World}", "Hello World")]
        [TestCase("str.trimStart();", " Hello World ", "Hello World ")]
        [TestCase("str.trimStart(['{', '}']);", "{Hello World}", "Hello World}")]
        [TestCase("str.trimEnd();", " Hello World ", " Hello World")]
        [TestCase("str.trimEnd(['{', '}']);", "{Hello World}", "{Hello World")]
        public void TrimWorks(string script, string input, string expectedResult)
        {
            var result = ScriptProcessorFactory.Run($"var str = new String('{input}');" + script);

            Assert.That(result, Is.InstanceOf<SString>());
            Assert.That(((SString)result).Value, Is.EqualTo(expectedResult));
        }

        [TestCase("str.toUpper();", "HELLO WORLD")]
        [TestCase("str.toLower();", "hello world")]
        public void Casing(string function, string expectedResult)
        {
            var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); " + function);

            Assert.That(result, Is.InstanceOf<SString>());
            Assert.That(((SString)result).Value, Is.EqualTo(expectedResult));
        }

        [TestCase("str.split();", "Hello,World", new[] { "Hello,World" })]
        [TestCase(" str.split(',');", "Hello,World", new[] { "Hello", "World" })]
        [TestCase("str.split([',', ';']);", "Hello,World;Hi", new[] { "Hello", "World", "Hi" })]
        [TestCase("str.split(',', 2);", "H,e,l,l,o,W,o,r,l,d", new[] { "H", "e" })]
        public void Split(string function, string input, string[] expectedValues)
        {
            var result = ScriptProcessorFactory.Run($"var str = new String('{input}'); " + function);

            Assert.That(result, Is.InstanceOf<SArray>());

            var resultAsArray = (SArray) result;

            Assert.That(resultAsArray.ArrayMembers.Length, Is.EqualTo(expectedValues.Length));
            for (var i = 0; i < expectedValues.Length; i++)
            {
                Assert.That(((SString)resultAsArray.ArrayMembers[i]).Value, Is.EqualTo(expectedValues[i]));
            }
        }

        [TestCase("str.slice(6);", "Hello World", "World")]
        [TestCase("str.slice(20);", "Hello World", "")]
        [TestCase("str.slice(-3);", "Hello World", "rld")]
        [TestCase("str.slice(-20);", "Hello World", "Hello World")]
        [TestCase("str.slice(2, 6);", "Hello World", "llo ")]
        [TestCase("str.slice(0, 0);", "Hello World", "")]
        [TestCase("str.slice(2, -3);", "Hello World", "llo Wo")]
        [TestCase("str.slice(-4, -1);", "Hello World", "orl")]
        [TestCase("str.slice(-20, -1);", "Hello World", "Hello Worl")]
        public void Slice(string function, string input, string expectedResult)
        {
            var result = ScriptProcessorFactory.Run($"var str = new String('{input}'); " + function);

            Assert.That(result, Is.InstanceOf<SString>());
            Assert.That(((SString)result).Value, Is.EqualTo(expectedResult));
        }

        [TestCase("str.replace('Something', 'Hello');", "Something World", "Hello World")]
        [TestCase("str.replace('Something', 'Hello');", "Something World, Something!", "Hello World, Hello!")]
        [TestCase("str.replace('', 'Hello');", "Something World", "Something World")]
        [TestCase("str.replace('Something', '');", "Something World", " World")]
        public void Replace(string function, string input, string expectedResult)
        {
            var result = ScriptProcessorFactory.Run($"var str = new String('{input}'); " + function);

            Assert.That(result, Is.InstanceOf<SString>());
            Assert.That(((SString)result).Value, Is.EqualTo(expectedResult));
        }

        [TestCase("str.indexOf('');", "Hello World Hello World", -1)]
        [TestCase("str.indexOf('Hello');", "", -1)]
        [TestCase("str.indexOf('World');", "Hello World Hello World", 6)]
        [TestCase("str.indexOf('something');", "Hello World Hello World", -1)]
        public void IndexOf(string function, string input, double expectedResult)
        {
            var result = ScriptProcessorFactory.Run($"var str = new String('{input}'); " + function);

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(expectedResult));
        }

        [TestFixture]
        public class LastIndexOfTest
        {
            [Test]
            public void LastIndexOfEmptyString()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World Hello World'); str.lastIndexOf('');");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, -1);
            }

            [Test]
            public void LastIndexOfOnEmptyString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String(''); str.lastIndexOf('Hello');");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, -1);
            }

            [Test]
            public void LastIndexOfPositive()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World Hello World'); str.lastIndexOf('World');");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, 18);
            }

            [Test]
            public void LastIndexOfNegative()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World Hello World'); str.lastIndexOf('something');");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, -1);
            }
        }

        [TestFixture]
        public class StartsWithTest
        {
            [Test]
            public void StartsWithPositive()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.startsWith('Hello');");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
            }

            [Test]
            public void StartsWithNegative()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.startsWith('World');");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, false);
            }

            [Test]
            public void StartsWithEmptyString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.startsWith('');");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, false);
            }

            [Test]
            public void StartsWithEmptyStringOnEmptyString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String(''); str.startsWith('');");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
            }
        }

        [TestFixture]
        public class EndsWithTest
        {
            [Test]
            public void EndsWithPositive()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.endsWith('World');");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
            }

            [Test]
            public void EndsWithNegative()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.endsWith('Hello');");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, false);
            }

            [Test]
            public void EndsWithEmptyString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.endsWith('');");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, false);
            }

            [Test]
            public void EndsWithEmptyStringOnEmptyString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String(''); str.endsWith('');");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
            }
        }

        [TestFixture]
        public class IncludesTest
        {
            [Test]
            public void IncludesEmptyString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String(''); str.includes('');");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
            }

            [Test]
            public void IncludesContains()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.includes('Hello');");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
            }

            [Test]
            public void IncludesNotEqualTypes()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello 42 World'); str.includes(42);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
            }
        }

        [TestFixture]
        public class CharAtTest
        {
            [Test]
            public void CharAtString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.charAt(2);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "l");
            }

            [Test]
            public void CharAtEmptyString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String(''); str.charAt(0);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "");
            }

            [Test]
            public void CharAtOutOfRange()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.charAt(42);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "");
            }

            [Test]
            public void CharAtNegative()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.charAt(-42);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "");
            }

            [Test]
            public void CharAtNoArg()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.charAt();");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "H");
            }
        }

        [TestFixture]
        public class ConcatTest
        {
            [Test]
            public void ConcatTwoStrings()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello, '); str.concat('Kevin', ' have a nice day.');");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "Hello, Kevin have a nice day.");
            }

            [Test]
            public void ConcatNonString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello, '); str.concat(42, undefined);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "Hello, 42undefined");
            }
        }

        [TestFixture]
        public class RepeatTest
        {
            [Test]
            public void Repeat()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('A'); str.repeat(4);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "AAAA");
            }

            [Test]
            public void RepeatZero()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('A'); str.repeat(0);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "");
            }

            [Test]
            public void RepeatNegative()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('A'); str.repeat(-1);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "");
            }
        }

        [TestFixture]
        public class PaddingTest
        {
            [Test]
            public void PadStartLength()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('abc'); str.padStart(5);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "  abc");
            }

            [Test]
            public void PadStartCharacter()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('abc'); str.padStart(10, 'foo');");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "foofoofabc");
            }

            [Test]
            public void PadStartEmptyString()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('abc'); str.padStart(5, '');");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "abc");
            }

            [Test]
            public void PadStartNegative()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String(' Hello World'); str.padStart(-1);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, " Hello World");
            }

            [Test]
            public void PadEndLength()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('abc'); str.padEnd(5);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "abc  ");
            }

            [Test]
            public void PadEndCharacter()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('abc'); str.padEnd(10, 'foo');");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "abcfoofoof");
            }

            [Test]
            public void PadEndEmptyString()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World'); str.padEnd(2, '');");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "Hello World");
            }

            [Test]
            public void PadEndNegative()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World '); str.padEnd(-1);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "Hello World ");
            }
        }
    }
}
