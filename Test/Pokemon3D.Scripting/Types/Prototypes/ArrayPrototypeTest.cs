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
        public class IndexerTest
        {
            [TestMethod]
            public void IndexerGet()
            {
                var processor = ScriptProcessorFactory.GetNew();
                var result = processor.Run("var arr = [1, 2, 3]; arr[1];");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, 2);
            }

            [TestMethod]
            public void IndexerGetOutOfRange()
            {
                var processor = ScriptProcessorFactory.GetNew();
                var result = processor.Run("var arr = [1, 2, 3]; arr[3];");

                Assert.IsTrue(result is SUndefined);
            }

            [TestMethod]
            public void IndexerSet()
            {
                var processor = ScriptProcessorFactory.GetNew();
                var result = processor.Run("var arr = [1, 0, 3]; arr[1] = 2; arr[1];");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, 2);
            }

            [TestMethod]
            public void IndexerSetOutOfRange()
            {
                var processor = ScriptProcessorFactory.GetNew();
                var result = processor.Run("var arr = [1, 2, 3]; arr[3] = 4; arr[3];");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, 4);
            }

            [TestMethod]
            public void IndexerSetOutOfRangeAccessEmptyMember()
            {
                var processor = ScriptProcessorFactory.GetNew();
                var result = processor.Run("var arr = [1, 2, 3]; arr[4] = 5; arr[3];");

                Assert.IsTrue(result is SUndefined);
            }
        }

        [TestClass]
        public class LengthTest
        {
            [TestMethod]
            public void LengthEmptyArray()
            {
                var processor = ScriptProcessorFactory.GetNew();
                var result = processor.Run("var arr = []; arr.length;");

                Assert.IsTrue(result is SNumber);
                Assert.AreEqual(((SNumber)result).Value, 0);
            }

            [TestMethod]
            public void Length()
            {
                var processor = ScriptProcessorFactory.GetNew();
                var result = processor.Run("var arr = [1,,2]; arr.length;");

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
                var processor = ScriptProcessorFactory.GetNew();
                var result = processor.Run("var arr = [1, 2, 3]; arr.includes(1);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
            }

            [TestMethod]
            public void IncludesNotEqualType()
            {
                var processor = ScriptProcessorFactory.GetNew();
                var result = processor.Run("var arr = [1, \"2\", 3]; arr.includes(2);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
            }

            [TestMethod]
            public void IncludesCustomComparer()
            {
                var processor = ScriptProcessorFactory.GetNew();
                var result = processor.Run("var arr = [1, \"2\", 3]; arr.includes(2, (m, o => m === o));");

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
                var processor = ScriptProcessorFactory.GetNew();
                var result = processor.Run("var arr = [1, 2, 3]; arr.any(m => m == 1);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
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
                var processor = ScriptProcessorFactory.GetNew();
                var result = processor.Run("var arr = [1, \"2\", 3]; arr.any(m => m == undefined);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, false);
            }

            [TestMethod]
            public void AnyUndefined()
            {
                var processor = ScriptProcessorFactory.GetNew();
                var result = processor.Run("var arr = [1, \"2\", 3, undefined]; arr.any(m => m == undefined);");

                Assert.IsTrue(result is SBool);
                Assert.AreEqual(((SBool)result).Value, true);
            }
        }
    }
}
