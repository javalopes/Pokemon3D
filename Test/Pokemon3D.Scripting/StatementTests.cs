using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pokemon3D.Scripting.Types;

namespace Test.Pokemon3D.Scripting
{
    [TestClass]
    public class StatementTests
    {
        [TestMethod]
        public void IfTests()
        {
            var result = ScriptProcessorFactory.Run("var a = 0; if (a == 0) { a = 5; } a;");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 5);
        }

        [TestMethod]
        public void IfElseTests()
        {
            var result = ScriptProcessorFactory.Run("var a = 0; if (a > 0) { a = 5; } else { a = 10; } a;");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 10);
        }

        [TestMethod]
        public void ForLoopTests()
        {
            var result = ScriptProcessorFactory.Run("var a = 0; for (var i = 0; i < 10; i++) { a++; } a;");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 10);
        }

        [TestMethod]
        public void WhileLoopTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("var a = 0; while (a < 10) { a++; } a;");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 10);
        }

        [TestMethod]
        public void ContinueTests()
        {
            var result = ScriptProcessorFactory.Run("var a = 0; for (var i = 0; i < 10; i++) { if (i % 2 == 0) { a++; } } a;");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 5);
        }

        [TestMethod]
        public void BreakTests()
        {
            var result = ScriptProcessorFactory.Run("var a = 0; for (var i = 0; i < 10; i++) { if (i > 5) { break; } a++; } a;");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 6);
        }
    }
}
