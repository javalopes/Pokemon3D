using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pokemon3D.Scripting.Types;

namespace Test.Pokemon3D.Scripting.Types.Prototypes
{
    public class ArrayPrototypeTest
    {
        [TestClass]
        public class ArrayTests
        {
            [TestMethod]
            public void EmptyArrayInitialize()
            {
                var result = ScriptProcessorFactory.Run("var arr = []; arr;");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(0, ((SArray)result).ArrayMembers.Length);
            }

            [TestMethod]
            public void ArrayLengthInitializer()
            {
                var result = ScriptProcessorFactory.Run("var arr = new Array(2); arr;");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(2, ((SArray)result).ArrayMembers.Length);
            }

            [TestMethod]
            public void SingleElementArrayInitialize()
            {
                var result = ScriptProcessorFactory.Run("var arr = [3]; arr;");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(1, ((SArray)result).ArrayMembers.Length);
                Assert.IsTrue(((SArray)result).ArrayMembers[0] is SNumber);
            }
        }

        [TestClass]
        public class IndexerTest
        {
            [TestMethod]
            public void IndexerGet()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr[1];");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(2, ((SNumber)result).Value);
            }

            [TestMethod]
            public void IndexerGetOutOfRange()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr[3];");

                Assert.IsTrue(result is SUndefined);
            }

            [TestMethod]
            public void IndexerSet()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 0, 3]; arr[1] = 2; arr[1];");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(2, ((SNumber)result).Value);
            }

            [TestMethod]
            public void IndexerSetOutOfRange()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr[3] = 4; arr[3];");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(4, ((SNumber)result).Value);
            }

            [TestMethod]
            public void IndexerSetOutOfRangeAccessEmptyMember()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr[4] = 5; arr[3];");

                Assert.IsTrue(result is SUndefined);
            }
        }

        [TestClass]
        public class LengthTest
        {
            [TestMethod]
            public void LengthEmptyArray()
            {
                var result = ScriptProcessorFactory.Run("var arr = []; arr.length;");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(0, ((SNumber)result).Value);
            }

            [TestMethod]
            public void Length()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1,,2]; arr.length;");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(3, ((SNumber)result).Value);
            }
        }

        [TestClass]
        public class IncludesTest
        {
            [TestMethod]
            public void IncludesContains()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.includes(1);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(true, ((SBool)result).Value);
            }

            [TestMethod]
            public void IncludesNotEqualType()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, \"2\", 3]; arr.includes(2);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(true, ((SBool)result).Value);
            }

            [TestMethod]
            public void IncludesCustomComparer()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, \"2\", 3]; arr.includes(2, (m, o => m === o));");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(false, ((SBool)result).Value);
            }
        }

        [TestClass]
        public class AnyTest
        {
            [TestMethod]
            public void AnyContains()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.any(m => m == 1);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(true, ((SBool)result).Value);
            }

            [TestMethod]
            public void AnyNoCondition()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.any();");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(true, ((SBool)result).Value);
            }

            [TestMethod]
            public void AnyNoConditionEmptyArray()
            {
                var result = ScriptProcessorFactory.Run("var arr = []; arr.any();");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(false, ((SBool)result).Value);
            }

            [TestMethod]
            public void AnyEmptyArray()
            {
                var processor = ScriptProcessorFactory.GetNew();
                var result = processor.Run("var arr = []; arr.any(m => m == 1);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(false, ((SBool)result).Value);
            }

            [TestMethod]
            public void AnyUndefinedCheck()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, \"2\", 3]; arr.any(m => m == undefined);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(false, ((SBool)result).Value);
            }

            [TestMethod]
            public void AnyUndefined()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, \"2\", 3, undefined]; arr.any(m => m == undefined);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(true, ((SBool)result).Value);
            }
        }

        [TestClass]
        public class WhereTest
        {
            [TestMethod]
            public void WhereNoResult()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.where(m => m > 3);");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(0, ((SArray)result).ArrayMembers.Length);
            }

            [TestMethod]
            public void Where()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.where(m => m > 1);");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(2, ((SArray)result).ArrayMembers.Length);
            }
        }

        [TestClass]
        public class SelectTest
        {
            [TestMethod]
            public void SelectIntManipulation()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.select(m => m + 2);");

                Assert.IsTrue(result is SArray);

                var arr = (SArray)result;
                Assert.AreEqual(3, arr.ArrayMembers.Length);
                Assert.IsTrue(arr.ArrayMembers[0] is SNumber);
                Assert.IsTrue(arr.ArrayMembers[1] is SNumber);
                Assert.IsTrue(arr.ArrayMembers[2] is SNumber);

                Assert.AreEqual(3, ((SNumber)arr.ArrayMembers[0]).Value);
                Assert.AreEqual(4, ((SNumber)arr.ArrayMembers[1]).Value);
                Assert.AreEqual(5, ((SNumber)arr.ArrayMembers[2]).Value);
            }

            [TestMethod]
            public void SelectWithUndefined()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, undefined, 3]; arr.select(m => m + 2);");

                Assert.IsTrue(result is SArray);

                var arr = (SArray)result;
                Assert.AreEqual(arr.ArrayMembers.Length, 3);
                Assert.IsTrue(arr.ArrayMembers[0] is SNumber);
                Assert.IsTrue(arr.ArrayMembers[1] is SNumber);
                Assert.IsTrue(arr.ArrayMembers[2] is SNumber);

                Assert.AreEqual(3, ((SNumber)arr.ArrayMembers[0]).Value);
                Assert.AreEqual(double.NaN, ((SNumber)arr.ArrayMembers[1]).Value);
                Assert.AreEqual(5, ((SNumber)arr.ArrayMembers[2]).Value);
            }
        }

        [TestClass]
        public class SingleTest
        {
            [TestMethod]
            public void SingleSingleRecord()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1]; arr.single();");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(1, ((SNumber)result).Value);
            }

            [TestMethod]
            public void SingleSingleRecordWithFilter()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.single(m => m == 2);");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(2, ((SNumber)result).Value);
            }

            [TestMethod]
            public void SingleMultipleRecords()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.single();");

                Assert.IsTrue(result is SError);
            }

            [TestMethod]
            public void SingleMultipleRecordsWithFilter()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.single(m => m == 1);");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(1, ((SNumber)result).Value);
            }
        }

        [TestClass]
        public class FirstTest
        {
            [TestMethod]
            public void FirstNoRecord()
            {
                var result = ScriptProcessorFactory.Run("var arr = []; arr.first();");

                Assert.IsTrue(result is SUndefined);
            }

            [TestMethod]
            public void FirstSingleRecord()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1]; arr.first();");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(1, ((SNumber)result).Value);
            }

            [TestMethod]
            public void First()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.first();");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(1, ((SNumber)result).Value);
            }

            [TestMethod]
            public void FirstWithFilter()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.first(m => m > 1);");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(2, ((SNumber)result).Value);
            }

            [TestMethod]
            public void FirstWithFilterNoResult()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.first(m => m > 3);");

                Assert.IsTrue(result is SUndefined);
            }
        }

        [TestClass]
        public class LastTest
        {
            [TestMethod]
            public void LastNoRecord()
            {
                var result = ScriptProcessorFactory.Run("var arr = []; arr.last();");

                Assert.IsTrue(result is SUndefined);
            }

            [TestMethod]
            public void LastSingleRecord()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1]; arr.last();");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(1, ((SNumber)result).Value);
            }

            [TestMethod]
            public void Last()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.last();");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(3, ((SNumber)result).Value);
            }

            [TestMethod]
            public void LastWithFilter()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.last(m => m < 3);");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(2, ((SNumber)result).Value);
            }

            [TestMethod]
            public void LastWithFilterNoResult()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.last(m => m > 3);");

                Assert.IsTrue(result is SUndefined);
            }
        }

        [TestClass]
        public class CountTest
        {
            [TestMethod]
            public void CountEmpty()
            {
                var result = ScriptProcessorFactory.Run("var arr = []; arr.count();");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(0, ((SNumber)result).Value);
            }

            [TestMethod]
            public void Count()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.count();");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(3, ((SNumber)result).Value);
            }

            [TestMethod]
            public void CountWithFilter()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.count(m => m > 1);");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(2, ((SNumber)result).Value);
            }
        }
    }
}
