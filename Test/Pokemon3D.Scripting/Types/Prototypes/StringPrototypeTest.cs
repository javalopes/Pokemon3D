using NUnit.Framework;
using Pokemon3D.Scripting.Types;

namespace Test.Pokemon3D.Scripting.Types.Prototypes
{
    public class StringPrototypeTest
    {
        [TestFixture]
        public class LengthTest
        {
            [Test]
            public void StringLength()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.length;");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual("Hello World".Length, ((SNumber)result).Value);
            }

            [Test]
            public void EmptyString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String(''); str.length;");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(0, ((SNumber)result).Value);
            }
        }

        [TestFixture]
        public class EmptyTest
        {
            [Test]
            public void Empty()
            {
                var result = ScriptProcessorFactory.Run("var str = String.empty; str;");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("", ((SString)result).Value);
            }
        }

        [TestFixture]
        public class RemoveTest
        {
            [Test]
            public void RemoveBegin()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.remove(6);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello ", ((SString)result).Value);
            }

            [Test]
            public void RemoveBeginOutOfRange()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.remove(20);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello World", ((SString)result).Value);
            }

            [Test]
            public void RemoveBeginNegative()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.remove(-1);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("", ((SString)result).Value);
            }

            [Test]
            public void RemoveBeginEnd()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.remove(0, 4);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("o World", ((SString)result).Value);
            }

            [Test]
            public void RemoveBeginEndOutOfRange()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.remove(0, 20);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("", ((SString)result).Value);
            }

            [Test]
            public void RemoveBeginEndNegative()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.remove(0, -1);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello World", ((SString)result).Value);
            }
        }

        [TestFixture]
        public class TrimTest
        {
            [Test]
            public void Trim()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String(' Hello World '); str.trim();");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello World", ((SString)result).Value);
            }

            [Test]
            public void TrimCharacters()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('{Hello World}'); str.trim(['{', '}']);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello World", ((SString)result).Value);
            }

            [Test]
            public void TrimStart()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String(' Hello World '); str.trimStart();");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello World ", ((SString)result).Value);
            }

            [Test]
            public void TrimStartCharacters()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('{Hello World}'); str.trimStart(['{', '}']);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello World}", ((SString)result).Value);
            }

            [Test]
            public void TrimEnd()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String(' Hello World '); str.trimEnd();");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(" Hello World", ((SString)result).Value);
            }

            [Test]
            public void TrimEndCharacters()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('{Hello World}'); str.trimEnd(['{', '}']);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("{Hello World", ((SString)result).Value);
            }
        }

        [TestFixture]
        public class CasingTest
        {
            [Test]
            public void ToUpper()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.toUpper();");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("HELLO WORLD", ((SString)result).Value);
            }

            [Test]
            public void ToLower()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.toLower();");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("hello world", ((SString)result).Value);
            }
        }

        [TestFixture]
        public class SplitTest
        {
            [Test]
            public void SplitEmptyArgs()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello,World'); str.split();");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(1, ((SArray)result).ArrayMembers.Length);
                Assert.IsTrue(((SArray)result).ArrayMembers[0] is SString);
                Assert.AreEqual("Hello,World", ((SString)((SArray)result).ArrayMembers[0]).Value);
            }

            [Test]
            public void SplitComma()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello,World'); str.split(',');");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(2, ((SArray)result).ArrayMembers.Length);
                Assert.IsTrue(((SArray)result).ArrayMembers[0] is SString);
                Assert.IsTrue(((SArray)result).ArrayMembers[1] is SString);
                Assert.AreEqual("Hello", ((SString)((SArray)result).ArrayMembers[0]).Value);
                Assert.AreEqual("World", ((SString)((SArray)result).ArrayMembers[1]).Value);
            }

            [Test]
            public void SplitMultiple()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello,World;Hi'); str.split([',', ';']);");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(3, ((SArray)result).ArrayMembers.Length);
                Assert.IsTrue(((SArray)result).ArrayMembers[0] is SString);
                Assert.IsTrue(((SArray)result).ArrayMembers[1] is SString);
                Assert.IsTrue(((SArray)result).ArrayMembers[2] is SString);
                Assert.AreEqual("Hello", ((SString)((SArray)result).ArrayMembers[0]).Value);
                Assert.AreEqual("World", ((SString)((SArray)result).ArrayMembers[1]).Value);
                Assert.AreEqual("Hi", ((SString)((SArray)result).ArrayMembers[2]).Value);
            }

            [Test]
            public void SplitWithLimit()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('H,e,l,l,o,W,o,r,l,d'); str.split(',', 2);");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(2, ((SArray)result).ArrayMembers.Length);
                Assert.IsTrue(((SArray)result).ArrayMembers[0] is SString);
                Assert.IsTrue(((SArray)result).ArrayMembers[1] is SString);
                Assert.AreEqual("H", ((SString)((SArray)result).ArrayMembers[0]).Value);
                Assert.AreEqual("e", ((SString)((SArray)result).ArrayMembers[1]).Value);
            }
        }

        [TestFixture]
        public class SliceTest
        {
            [Test]
            public void SliceBegin()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World'); str.slice(6);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("World", ((SString)result).Value);
            }

            [Test]
            public void SliceBeginOutOfRange()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World'); str.slice(20);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("", ((SString)result).Value);
            }

            [Test]
            public void SliceBeginNegative()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World'); str.slice(-3);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("rld", ((SString)result).Value);
            }

            [Test]
            public void SliceBeginNegativeOutOfRange()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World'); str.slice(-20);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello World", ((SString)result).Value);
            }

            [Test]
            public void SliceBeginEnd()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World'); str.slice(2, 6);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("llo ", ((SString)result).Value);
            }

            [Test]
            public void SliceBeginEndZero()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World'); str.slice(0, 0);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("", ((SString)result).Value);
            }

            [Test]
            public void SliceBeginPositiveEndNegative()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World'); str.slice(2, -3);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("llo Wo", ((SString)result).Value);
            }

            [Test]
            public void SliceBeginNegativeEndNegative()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World'); str.slice(-4, -1);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("orl", ((SString)result).Value);
            }

            [Test]
            public void SliceBeginNegativeEndNegativeOutOfRange()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World'); str.slice(-20, -1);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello Worl", ((SString)result).Value);
            }
        }

        [TestFixture]
        public class ReplaceTest
        {
            [Test]
            public void Replace()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Something World'); str.replace('Something', 'Hello');");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello World", ((SString)result).Value);
            }

            [Test]
            public void ReplaceMultiple()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Something World, Something!'); str.replace('Something', 'Hello');");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello World, Hello!", ((SString)result).Value);
            }

            [Test]
            public void ReplaceEmptyString()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Something World'); str.replace('', 'Hello');");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Something World", ((SString)result).Value);
            }

            [Test]
            public void ReplaceWithEmptyString()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Something World'); str.replace('Something', '');");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(" World", ((SString)result).Value);
            }
        }

        [TestFixture]
        public class IndexOfTest
        {
            [Test]
            public void IndexOfEmptyString()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World Hello World'); str.indexOf('');");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, -1);
            }

            [Test]
            public void IndexOfOnEmptyString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String(''); str.indexOf('Hello');");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, -1);
            }

            [Test]
            public void IndexOfPositive()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World Hello World'); str.indexOf('World');");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, 6);
            }

            [Test]
            public void IndexOfNegative()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World Hello World'); str.indexOf('something');");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, -1);
            }
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
