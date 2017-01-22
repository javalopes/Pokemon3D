using NUnit.Framework;
using Pokemon3D.Scripting.Types;

namespace Test.Pokemon3D.Scripting.Types.Prototypes
{
    public class ArrayPrototypeTest
    {
        [TestFixture]
        public class ArrayTests
        {
            [Test]
            public void EmptyArrayInitialize()
            {
                var result = ScriptProcessorFactory.Run("var arr = []; arr;");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(0, ((SArray)result).ArrayMembers.Length);
            }

            [Test]
            public void ArrayLengthInitializer()
            {
                var result = ScriptProcessorFactory.Run("var arr = new Array(2); arr;");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(2, ((SArray)result).ArrayMembers.Length);
            }

            [Test]
            public void SingleElementArrayInitialize()
            {
                var result = ScriptProcessorFactory.Run("var arr = [3]; arr;");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(1, ((SArray)result).ArrayMembers.Length);
                Assert.IsTrue(((SArray)result).ArrayMembers[0] is SNumber);
            }
        }

        [TestFixture]
        public class IndexerTest
        {
            [Test]
            public void IndexerGet()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr[1];");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(2, ((SNumber)result).Value);
            }

            [Test]
            public void IndexerGetOutOfRange()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr[3];");

                Assert.IsTrue(result is SUndefined);
            }

            [Test]
            public void IndexerSet()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 0, 3]; arr[1] = 2; arr[1];");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(2, ((SNumber)result).Value);
            }

            [Test]
            public void IndexerSetOutOfRange()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr[3] = 4; arr[3];");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(4, ((SNumber)result).Value);
            }

            [Test]
            public void IndexerSetOutOfRangeAccessEmptyMember()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr[4] = 5; arr[3];");

                Assert.IsTrue(result is SUndefined);
            }
        }

        [TestFixture]
        public class LengthTest
        {
            [Test]
            public void LengthEmptyArray()
            {
                var result = ScriptProcessorFactory.Run("var arr = []; arr.length;");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(0, ((SNumber)result).Value);
            }

            [Test]
            public void Length()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1,,2]; arr.length;");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(3, ((SNumber)result).Value);
            }
        }

        [TestFixture]
        public class IncludesTest
        {
            [Test]
            public void IncludesContains()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.includes(1);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(true, ((SBool)result).Value);
            }

            [Test]
            public void IncludesNotEqualType()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, \"2\", 3]; arr.includes(2);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(true, ((SBool)result).Value);
            }

            [Test]
            public void IncludesCustomComparer()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, \"2\", 3]; arr.includes(2, (m, o => m === o));");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(false, ((SBool)result).Value);
            }
        }

        [TestFixture]
        public class AnyTest
        {
            [Test]
            public void AnyContains()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.any(m => m == 1);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(true, ((SBool)result).Value);
            }

            [Test]
            public void AnyNoCondition()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.any();");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(true, ((SBool)result).Value);
            }

            [Test]
            public void AnyNoConditionEmptyArray()
            {
                var result = ScriptProcessorFactory.Run("var arr = []; arr.any();");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(false, ((SBool)result).Value);
            }

            [Test]
            public void AnyEmptyArray()
            {
                var processor = ScriptProcessorFactory.GetNew();
                var result = processor.Run("var arr = []; arr.any(m => m == 1);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(false, ((SBool)result).Value);
            }

            [Test]
            public void AnyUndefinedCheck()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, \"2\", 3]; arr.any(m => m == undefined);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(false, ((SBool)result).Value);
            }

            [Test]
            public void AnyUndefined()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, \"2\", 3, undefined]; arr.any(m => m == undefined);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(true, ((SBool)result).Value);
            }
        }

        [TestFixture]
        public class AllTest
        {
            [Test]
            public void AllContains()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.all(m => m > 0);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(true, ((SBool)result).Value);
            }
            
            [Test]
            public void AllEmptyArray()
            {
                var processor = ScriptProcessorFactory.GetNew();
                var result = processor.Run("var arr = []; arr.all(m => m == 1);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(true, ((SBool)result).Value);
            }

            [Test]
            public void AllUndefinedCheck()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, \"2\", 3]; arr.all(m => m != undefined);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(true, ((SBool)result).Value);
            }

            [Test]
            public void AllUndefined()
            {
                var result = ScriptProcessorFactory.Run("var arr = [undefined,,,undefined]; arr.all(m => m == undefined);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(true, ((SBool)result).Value);
            }
        }

        [TestFixture]
        public class WhereTest
        {
            [Test]
            public void WhereNoResult()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.where(m => m > 3);");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(0, ((SArray)result).ArrayMembers.Length);
            }

            [Test]
            public void Where()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.where(m => m > 1);");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(2, ((SArray)result).ArrayMembers.Length);
            }
        }

        [TestFixture]
        public class SelectTest
        {
            [Test]
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

            [Test]
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

        [TestFixture]
        public class SingleTest
        {
            [Test]
            public void SingleSingleRecord()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1]; arr.single();");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(1, ((SNumber)result).Value);
            }

            [Test]
            public void SingleSingleRecordWithFilter()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.single(m => m == 2);");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(2, ((SNumber)result).Value);
            }

            [Test]
            public void SingleMultipleRecords()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.single();");

                Assert.IsTrue(result is SError);
            }

            [Test]
            public void SingleMultipleRecordsWithFilter()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.single(m => m == 1);");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(1, ((SNumber)result).Value);
            }
        }

        [TestFixture]
        public class FirstTest
        {
            [Test]
            public void FirstNoRecord()
            {
                var result = ScriptProcessorFactory.Run("var arr = []; arr.first();");

                Assert.IsTrue(result is SUndefined);
            }

            [Test]
            public void FirstSingleRecord()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1]; arr.first();");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(1, ((SNumber)result).Value);
            }

            [Test]
            public void First()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.first();");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(1, ((SNumber)result).Value);
            }

            [Test]
            public void FirstWithFilter()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.first(m => m > 1);");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(2, ((SNumber)result).Value);
            }

            [Test]
            public void FirstWithFilterNoResult()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.first(m => m > 3);");

                Assert.IsTrue(result is SUndefined);
            }
        }

        [TestFixture]
        public class LastTest
        {
            [Test]
            public void LastNoRecord()
            {
                var result = ScriptProcessorFactory.Run("var arr = []; arr.last();");

                Assert.IsTrue(result is SUndefined);
            }

            [Test]
            public void LastSingleRecord()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1]; arr.last();");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(1, ((SNumber)result).Value);
            }

            [Test]
            public void Last()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.last();");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(3, ((SNumber)result).Value);
            }

            [Test]
            public void LastWithFilter()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.last(m => m < 3);");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(2, ((SNumber)result).Value);
            }

            [Test]
            public void LastWithFilterNoResult()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.last(m => m > 3);");

                Assert.IsTrue(result is SUndefined);
            }
        }

        [TestFixture]
        public class CountTest
        {
            [Test]
            public void CountEmpty()
            {
                var result = ScriptProcessorFactory.Run("var arr = []; arr.count();");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(0, ((SNumber)result).Value);
            }

            [Test]
            public void Count()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.count();");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(3, ((SNumber)result).Value);
            }

            [Test]
            public void CountWithFilter()
            {
                var result = ScriptProcessorFactory.Run("var arr = [1, 2, 3]; arr.count(m => m > 1);");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(2, ((SNumber)result).Value);
            }
        }

        [TestFixture]
        public class PushTest
        {
            [Test]
            public void PushSingleItem()
            {
                var result = ScriptProcessorFactory.Run("var arr = []; var item = 23; arr.push(item); arr;");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(1, ((SArray)result).ArrayMembers.Length);
                Assert.AreEqual(23, ((SNumber) ((SArray)result).ArrayMembers[0]).Value);
            }

            [Test]
            public void PushArray()
            {
                var result = ScriptProcessorFactory.Run("var arr = [23]; var items = [1, 2, 3]; arr.push(items); arr;");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(4, ((SArray)result).ArrayMembers.Length);
                Assert.AreEqual(2, ((SNumber) ((SArray)result).ArrayMembers[2]).Value);
            }

            [Test]
            public void PushMultiple()
            {
                var result = ScriptProcessorFactory.Run("var arr = [23]; var items = [1, 2, 3]; var item = 32; arr.push(items, item); arr;");

                Assert.IsTrue(result is SArray);
                Assert.AreEqual(5, ((SArray)result).ArrayMembers.Length);
                Assert.AreEqual(32, ((SNumber) ((SArray)result).ArrayMembers[4]).Value);
            }
        }

        [TestFixture]
        public class PopTest
        {
            [Test]
            public void PopItem()
            {
                var result = ScriptProcessorFactory.Run("var arr = [32]; arr.pop();");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(32, ((SNumber)result).Value);
            }

            [Test]
            public void PopEmpty()
            {
                var result = ScriptProcessorFactory.Run("var arr = []; arr.pop();");

                Assert.IsTrue(result is SUndefined);
            }
        }
    }
}
