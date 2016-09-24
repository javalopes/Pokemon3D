using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pokemon3D.Scripting.Types;

namespace Test.Pokemon3D.Scripting.Types.Prototypes
{
    public class StringPrototypeTest
    {
        [TestClass]
        public class LengthTest
        {
            [TestMethod]
            public void StringLength()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.length;");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual("Hello World".Length, ((SNumber)result).Value);
            }

            [TestMethod]
            public void EmptyString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String(''); str.length;");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(0, ((SNumber)result).Value);
            }
        }

        [TestClass]
        public class EmptyTest
        {
            [TestMethod]
            public void Empty()
            {
                var result = ScriptProcessorFactory.Run("var str = String.empty; str;");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("", ((SString)result).Value);
            }
        }

        [TestClass]
        public class RemoveTest
        {
            [TestMethod]
            public void RemoveBegin()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.remove(6);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello ", ((SString)result).Value);
            }

            [TestMethod]
            public void RemoveBeginOutOfRange()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.remove(20);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello World", ((SString)result).Value);
            }

            [TestMethod]
            public void RemoveBeginNegative()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.remove(-1);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("", ((SString)result).Value);
            }

            [TestMethod]
            public void RemoveBeginEnd()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.remove(0, 4);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("o World", ((SString)result).Value);
            }

            [TestMethod]
            public void RemoveBeginEndOutOfRange()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.remove(0, 20);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("", ((SString)result).Value);
            }

            [TestMethod]
            public void RemoveBeginEndNegative()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.remove(0, -1);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello World", ((SString)result).Value);
            }
        }

        [TestClass]
        public class TrimTest
        {
            [TestMethod]
            public void Trim()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String(' Hello World '); str.trim();");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello World", ((SString)result).Value);
            }

            [TestMethod]
            public void TrimCharacters()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('{Hello World}'); str.trim(['{', '}']);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello World", ((SString)result).Value);
            }

            [TestMethod]
            public void TrimStart()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String(' Hello World '); str.trimStart();");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello World ", ((SString)result).Value);
            }

            [TestMethod]
            public void TrimStartCharacters()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('{Hello World}'); str.trimStart(['{', '}']);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello World}", ((SString)result).Value);
            }

            [TestMethod]
            public void TrimEnd()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String(' Hello World '); str.trimEnd();");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(" Hello World", ((SString)result).Value);
            }

            [TestMethod]
            public void TrimEndCharacters()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('{Hello World}'); str.trimEnd(['{', '}']);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("{Hello World", ((SString)result).Value);
            }
        }

        [TestClass]
        public class CasingTest
        {
            [TestMethod]
            public void ToUpper()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.toUpper();");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("HELLO WORLD", ((SString)result).Value);
            }

            [TestMethod]
            public void ToLower()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.toLower();");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("hello world", ((SString)result).Value);
            }
        }

        [TestClass]
        public class SplitTest
        {
            [TestMethod]
            public void SplitEmptyArgs()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello,World'); str.split();");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(1, ((SArray)result).ArrayMembers.Length);
                Assert.IsTrue(((SArray)result).ArrayMembers[0] is SString);
                Assert.AreEqual("Hello,World", ((SString)((SArray)result).ArrayMembers[0]).Value);
            }

            [TestMethod]
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

            [TestMethod]
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

            [TestMethod]
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

        [TestClass]
        public class SliceTest
        {
            [TestMethod]
            public void SliceBegin()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World'); str.slice(6);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("World", ((SString)result).Value);
            }

            [TestMethod]
            public void SliceBeginOutOfRange()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World'); str.slice(20);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("", ((SString)result).Value);
            }

            [TestMethod]
            public void SliceBeginNegative()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World'); str.slice(-3);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("rld", ((SString)result).Value);
            }

            [TestMethod]
            public void SliceBeginNegativeOutOfRange()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World'); str.slice(-20);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello World", ((SString)result).Value);
            }

            [TestMethod]
            public void SliceBeginEnd()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World'); str.slice(2, 6);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("llo ", ((SString)result).Value);
            }

            [TestMethod]
            public void SliceBeginEndZero()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World'); str.slice(0, 0);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("", ((SString)result).Value);
            }

            [TestMethod]
            public void SliceBeginPositiveEndNegative()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World'); str.slice(2, -3);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("llo Wo", ((SString)result).Value);
            }

            [TestMethod]
            public void SliceBeginNegativeEndNegative()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World'); str.slice(-4, -1);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("orl", ((SString)result).Value);
            }

            [TestMethod]
            public void SliceBeginNegativeEndNegativeOutOfRange()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World'); str.slice(-20, -1);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello Worl", ((SString)result).Value);
            }
        }

        [TestClass]
        public class ReplaceTest
        {
            [TestMethod]
            public void Replace()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Something World'); str.replace('Something', 'Hello');");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello World", ((SString)result).Value);
            }

            [TestMethod]
            public void ReplaceMultiple()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Something World, Something!'); str.replace('Something', 'Hello');");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Hello World, Hello!", ((SString)result).Value);
            }

            [TestMethod]
            public void ReplaceEmptyString()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Something World'); str.replace('', 'Hello');");

                Assert.IsTrue(result is SString);
                Assert.AreEqual("Something World", ((SString)result).Value);
            }

            [TestMethod]
            public void ReplaceWithEmptyString()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Something World'); str.replace('Something', '');");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(" World", ((SString)result).Value);
            }
        }

        [TestClass]
        public class IndexOfTest
        {
            [TestMethod]
            public void IndexOfEmptyString()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World Hello World'); str.indexOf('');");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, -1);
            }

            [TestMethod]
            public void IndexOfOnEmptyString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String(''); str.indexOf('Hello');");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, -1);
            }

            [TestMethod]
            public void IndexOfPositive()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World Hello World'); str.indexOf('World');");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, 6);
            }

            [TestMethod]
            public void IndexOfNegative()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World Hello World'); str.indexOf('something');");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, -1);
            }
        }

        [TestClass]
        public class LastIndexOfTest
        {
            [TestMethod]
            public void LastIndexOfEmptyString()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World Hello World'); str.lastIndexOf('');");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, -1);
            }

            [TestMethod]
            public void LastIndexOfOnEmptyString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String(''); str.lastIndexOf('Hello');");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, -1);
            }

            [TestMethod]
            public void LastIndexOfPositive()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World Hello World'); str.lastIndexOf('World');");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, 18);
            }

            [TestMethod]
            public void LastIndexOfNegative()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World Hello World'); str.lastIndexOf('something');");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, -1);
            }
        }

        [TestClass]
        public class StartsWithTest
        {
            [TestMethod]
            public void StartsWithPositive()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.startsWith('Hello');");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
            }

            [TestMethod]
            public void StartsWithNegative()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.startsWith('World');");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, false);
            }

            [TestMethod]
            public void StartsWithEmptyString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.startsWith('');");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, false);
            }

            [TestMethod]
            public void StartsWithEmptyStringOnEmptyString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String(''); str.startsWith('');");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
            }
        }

        [TestClass]
        public class EndsWithTest
        {
            [TestMethod]
            public void EndsWithPositive()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.endsWith('World');");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
            }

            [TestMethod]
            public void EndsWithNegative()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.endsWith('Hello');");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, false);
            }

            [TestMethod]
            public void EndsWithEmptyString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.endsWith('');");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, false);
            }

            [TestMethod]
            public void EndsWithEmptyStringOnEmptyString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String(''); str.endsWith('');");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
            }
        }

        [TestClass]
        public class IncludesTest
        {
            [TestMethod]
            public void IncludesEmptyString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String(''); str.includes('');");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
            }

            [TestMethod]
            public void IncludesContains()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.includes('Hello');");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
            }

            [TestMethod]
            public void IncludesNotEqualTypes()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello 42 World'); str.includes(42);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
            }
        }

        [TestClass]
        public class CharAtTest
        {
            [TestMethod]
            public void CharAtString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.charAt(2);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "l");
            }

            [TestMethod]
            public void CharAtEmptyString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String(''); str.charAt(0);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "");
            }

            [TestMethod]
            public void CharAtOutOfRange()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.charAt(42);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "");
            }

            [TestMethod]
            public void CharAtNegative()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.charAt(-42);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "");
            }

            [TestMethod]
            public void CharAtNoArg()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello World'); str.charAt();");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "H");
            }
        }

        [TestClass]
        public class ConcatTest
        {
            [TestMethod]
            public void ConcatTwoStrings()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello, '); str.concat('Kevin', ' have a nice day.');");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "Hello, Kevin have a nice day.");
            }

            [TestMethod]
            public void ConcatNonString()
            {
                var result = ScriptProcessorFactory.Run("var str = new String('Hello, '); str.concat(42, undefined);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "Hello, 42undefined");
            }
        }

        [TestClass]
        public class RepeatTest
        {
            [TestMethod]
            public void Repeat()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('A'); str.repeat(4);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "AAAA");
            }

            [TestMethod]
            public void RepeatZero()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('A'); str.repeat(0);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "");
            }

            [TestMethod]
            public void RepeatNegative()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('A'); str.repeat(-1);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "");
            }
        }

        [TestClass]
        public class PaddingTest
        {
            [TestMethod]
            public void PadStartLength()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('abc'); str.padStart(5);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "  abc");
            }

            [TestMethod]
            public void PadStartCharacter()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('abc'); str.padStart(10, 'foo');");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "foofoofabc");
            }

            [TestMethod]
            public void PadStartEmptyString()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('abc'); str.padStart(5, '');");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "abc");
            }

            [TestMethod]
            public void PadStartNegative()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String(' Hello World'); str.padStart(-1);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, " Hello World");
            }

            [TestMethod]
            public void PadEndLength()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('abc'); str.padEnd(5);");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "abc  ");
            }

            [TestMethod]
            public void PadEndCharacter()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('abc'); str.padEnd(10, 'foo');");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "abcfoofoof");
            }

            [TestMethod]
            public void PadEndEmptyString()
            {
                var result = ScriptProcessorFactory
                    .Run("var str = new String('Hello World'); str.padEnd(2, '');");

                Assert.IsTrue(result is SString);
                Assert.AreEqual(((SString)result).Value, "Hello World");
            }

            [TestMethod]
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
