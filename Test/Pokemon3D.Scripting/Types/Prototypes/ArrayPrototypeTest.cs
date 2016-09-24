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
                Assert.AreEqual(((SArray)result).ArrayMembers.Length, 0);
            }

            [TestMethod]
            public void ArrayLengthInitializer()
            {
                var result = ScriptProcessorFactory.Run("var arr = new Array(2); arr;");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(((SArray)result).ArrayMembers.Length, 2);
            }

            [TestMethod]
            public void SingleElementArrayInitialize()
            {
                var result = ScriptProcessorFactory.Run("var arr = [3]; arr;");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(((SArray)result).ArrayMembers.Length, 1);
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
                Assert.AreEqual(((SNumber)result).Value, 2);
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
                Assert.AreEqual(((SNumber)result).Value, 2);
            }

            [TestMethod]
            public void IndexerSetOutOfRange()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr[3] = 4; arr[3];");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, 4);
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
                Assert.AreEqual(((SNumber)result).Value, 0);
            }

            [TestMethod]
            public void Length()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1,,2]; arr.length;");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, 3);
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
                Assert.AreEqual(((SBool)result).Value, true);
            }

            [TestMethod]
            public void IncludesNotEqualType()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, \"2\", 3]; arr.includes(2);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
            }

            [TestMethod]
            public void IncludesCustomComparer()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, \"2\", 3]; arr.includes(2, (m, o => m === o));");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, false);
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
                Assert.AreEqual(((SBool)result).Value, true);
            }

            [TestMethod]
            public void AnyNoCondition()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.any();");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
            }

            [TestMethod]
            public void AnyNoConditionEmptyArray()
            {
                var result = ScriptProcessorFactory.Run("var arr = []; arr.any();");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, false);
            }

            [TestMethod]
            public void AnyEmptyArray()
            {
                var processor = ScriptProcessorFactory.GetNew();
                var result = processor.Run("var arr = []; arr.any(m => m == 1);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, false);
            }

            [TestMethod]
            public void AnyUndefinedCheck()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, \"2\", 3]; arr.any(m => m == undefined);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, false);
            }

            [TestMethod]
            public void AnyUndefined()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, \"2\", 3, undefined]; arr.any(m => m == undefined);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
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
                Assert.AreEqual(((SArray)result).ArrayMembers.Length, 0);
            }

            [TestMethod]
            public void Where()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.where(m => m > 1);");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(((SArray)result).ArrayMembers.Length, 2);
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
                Assert.AreEqual(arr.ArrayMembers.Length, 3);
                Assert.IsTrue(arr.ArrayMembers[0] is SNumber);
                Assert.IsTrue(arr.ArrayMembers[1] is SNumber);
                Assert.IsTrue(arr.ArrayMembers[2] is SNumber);

                Assert.AreEqual(((SNumber)arr.ArrayMembers[0]).Value, 3);
                Assert.AreEqual(((SNumber)arr.ArrayMembers[1]).Value, 4);
                Assert.AreEqual(((SNumber)arr.ArrayMembers[2]).Value, 5);
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

                Assert.AreEqual(((SNumber)arr.ArrayMembers[0]).Value, 3);
                Assert.AreEqual(((SNumber)arr.ArrayMembers[1]).Value, double.NaN);
                Assert.AreEqual(((SNumber)arr.ArrayMembers[2]).Value, 5);
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
                Assert.AreEqual(((SNumber)result).Value, 1);
            }

            [TestMethod]
            public void SingleSingleRecordWithFilter()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.single(m => m == 2);");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, 2);
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
                Assert.AreEqual(((SNumber)result).Value, 1);
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
                Assert.AreEqual(((SNumber)result).Value, 1);
            }

            [TestMethod]
            public void First()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.first();");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, 1);
            }

            [TestMethod]
            public void FirstWithFilter()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.first(m => m > 1);");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, 2);
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
                Assert.AreEqual(((SNumber)result).Value, 1);
            }

            [TestMethod]
            public void Last()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.last();");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, 3);
            }

            [TestMethod]
            public void LastWithFilter()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.last(m => m < 3);");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, 2);
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
                Assert.AreEqual(((SNumber)result).Value, 0);
            }

            [TestMethod]
            public void Count()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.count();");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, 3);
            }

            [TestMethod]
            public void CountWithFilter()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.count(m => m > 1);");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, 2);
            }
        }
    }
}
