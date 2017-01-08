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

        [TestCase("str.lastIndexOf('');", "Hello World Hello World", -1)]
        [TestCase("str.lastIndexOf('Hello');", "", -1)]
        [TestCase("str.lastIndexOf('World');", "Hello World Hello World", 18)]
        [TestCase("str.lastIndexOf('something');", "Hello World Hello World", -1)]
        public void LastIndexOf(string function, string input, double expectedResult)
        {
            var result = ScriptProcessorFactory.Run($"var str = new String('{input}'); " + function);

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(expectedResult));
        }

        [TestCase("str.startsWith('Hello');", "Hello World", true)]
        [TestCase("str.startsWith('World');", "Hello World", false)]
        [TestCase("str.startsWith('');", "Hello World", false)]
        [TestCase("str.startsWith('');", "", true)]
        public void StartsWith(string function, string input, bool expectedResult)
        {
            var result = ScriptProcessorFactory.Run($"var str = new String('{input}'); " + function);

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.EqualTo(expectedResult));
        }

        [TestCase("str.endsWith('World');", "Hello World", true)]
        [TestCase("str.endsWith('Hello');", "Hello World", false)]
        [TestCase("str.endsWith('');", "Hello World", false)]
        [TestCase("str.endsWith('');", "", true)]
        public void EndsWith(string function, string input, bool expectedResult)
        {
            var result = ScriptProcessorFactory.Run($"var str = new String('{input}'); " + function);

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.EqualTo(expectedResult));
        }

        [TestCase("str.includes('');", "", true)]
        [TestCase("str.includes('Hello');", "Hello World", true)]
        [TestCase("str.includes('42');", "Hello 42 World", true)]
        public void Includes(string function, string input, bool expectedResult)
        {
            var result = ScriptProcessorFactory.Run($"var str = new String('{input}'); " + function);

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.EqualTo(expectedResult));
        }

        [TestCase("str.charAt(2);", "Hello World", "l")]
        [TestCase("str.charAt(0);", "", "")]
        [TestCase("str.charAt(42);", "Hello World", "")]
        [TestCase("str.charAt(-42);", "Hello World", "")]
        [TestCase("str.charAt();", "Hello World", "H")]
        public void CharAt(string function, string input, string expectedResult)
        {
            var result = ScriptProcessorFactory.Run($"var str = new String('{input}'); " + function);

            Assert.That(result, Is.InstanceOf<SString>());
            Assert.That(((SString)result).Value, Is.EqualTo(expectedResult));
        }

        [TestCase("str.concat('Kevin', ' have a nice day.');", "Hello, ", "Hello, Kevin have a nice day.")]
        [TestCase("str.concat(42, undefined);", "Hello, ", "Hello, 42undefined")]
        public void Concat(string function, string input, string expectedResult)
        {
            var result = ScriptProcessorFactory.Run($"var str = new String('{input}'); " + function);

            Assert.That(result, Is.InstanceOf<SString>());
            Assert.That(((SString)result).Value, Is.EqualTo(expectedResult));
        }

        [TestCase("str.repeat(4);", "A", "AAAA")]
        [TestCase("str.repeat(0);", "A", "")]
        [TestCase("str.repeat(-1);", "A", "")]
        public void Repeat(string function, string input, string expectedResult)
        {
            var result = ScriptProcessorFactory.Run($"var str = new String('{input}'); " + function);

            Assert.That(result, Is.InstanceOf<SString>());
            Assert.That(((SString)result).Value, Is.EqualTo(expectedResult));
        }

        [TestCase("str.padStart(5);", "abc", "  abc")]
        [TestCase("str.padStart(10, 'foo');", "abc", "foofoofabc")]
        [TestCase("str.padStart(5, '');", "abc", "abc")]
        [TestCase("str.padStart(-1);", " Hello World", " Hello World")]
        [TestCase("str.padEnd(5);", "abc", "abc  ")]
        [TestCase("str.padEnd(10, 'foo');", "abc", "abcfoofoof")]
        [TestCase("str.padEnd(2, '');", "Hello World", "Hello World")]
        [TestCase("str.padEnd(-1);", "Hello World ", "Hello World ")]
        public void Padding(string function, string input, string expectedResult)
        {
            var result = ScriptProcessorFactory.Run($"var str = new String('{input}'); " + function);

            Assert.That(result, Is.InstanceOf<SString>());
            Assert.That(((SString)result).Value, Is.EqualTo(expectedResult));

        }
    }
}
