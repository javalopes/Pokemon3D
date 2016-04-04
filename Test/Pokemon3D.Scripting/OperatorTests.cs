using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pokemon3D.Scripting.Types;

namespace Test.Pokemon3D.Scripting
{
    [TestClass]
    public class OperatorTests
    {
        [TestMethod]
        public void AdditionTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("1+4+6");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 11);
        }

        [TestMethod]
        public void SubtractionTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("12-4-6");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 2);
        }

        [TestMethod]
        public void MultiplicationTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("5*3*-4");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, -60);
        }

        [TestMethod]
        public void DivisionTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("15/5");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 3);
        }

        [TestMethod]
        public void IncrementTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("var a = 13; a++; a;");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 14);

        }

        [TestMethod]
        public void DecrementTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("var b = 13; b--; b;");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 12);
        }

        [TestMethod]
        public void PowerTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("2**3");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 8);
        }

        [TestMethod]
        public void ModuloTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("5%2");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 1);
        }

        [TestMethod]
        public void SmallerThanTests()
        {
            SObject result = null;
            var processor = ScriptProcessorFactory.GetNew();

            result = processor.Run("2 < 3");

            Assert.IsTrue(result is SBool);
            Assert.AreEqual(((SBool)result).Value, true);

            result = processor.Run("3 < -5");

            Assert.IsTrue(result is SBool);
            Assert.AreEqual(((SBool)result).Value, false);

            result = processor.Run("\"test\" < -5");

            Assert.IsTrue(result is SBool);
            Assert.AreEqual(((SBool)result).Value, false);
        }

        [TestMethod]
        public void SmallerThanOrEqualsTests()
        {
            SObject result = null;
            var processor = ScriptProcessorFactory.GetNew();

            result = processor.Run("2 <= 3");

            Assert.IsTrue(result is SBool);
            Assert.AreEqual(((SBool)result).Value, true);

            result = processor.Run("3 <= 3");

            Assert.IsTrue(result is SBool);
            Assert.AreEqual(((SBool)result).Value, true);

            result = processor.Run("\"test\" <= 0");

            Assert.IsTrue(result is SBool);
            Assert.AreEqual(((SBool)result).Value, false);
        }

        [TestMethod]
        public void GreaterThanTests()
        {
            SObject result = null;
            var processor = ScriptProcessorFactory.GetNew();

            result = processor.Run("3 > 2");

            Assert.IsTrue(result is SBool);
            Assert.AreEqual(((SBool)result).Value, true);

            result = processor.Run("1 > -5");

            Assert.IsTrue(result is SBool);
            Assert.AreEqual(((SBool)result).Value, true);

            result = processor.Run("\"test\" > -5");

            Assert.IsTrue(result is SBool);
            Assert.AreEqual(((SBool)result).Value, false);
        }

        [TestMethod]
        public void GreaterThanOrEqualsTests()
        {
            SObject result = null;
            var processor = ScriptProcessorFactory.GetNew();

            result = processor.Run("3 >= 2");

            Assert.IsTrue(result is SBool);
            Assert.AreEqual(((SBool)result).Value, true);

            result = processor.Run("3 >= 3");

            Assert.IsTrue(result is SBool);
            Assert.AreEqual(((SBool)result).Value, true);

            result = processor.Run("\"test\" >= 0");

            Assert.IsTrue(result is SBool);
            Assert.AreEqual(((SBool)result).Value, false);
        }

        [TestMethod]
        public void EqualsTests()
        {
            SObject result = null;
            var processor = ScriptProcessorFactory.GetNew();

            result = processor.Run("2 == 2");

            Assert.IsTrue(result is SBool);
            Assert.AreEqual(((SBool)result).Value, true);

            result = processor.Run("2 === 2");

            Assert.IsTrue(result is SBool);
            Assert.AreEqual(((SBool)result).Value, true);
        }

        [TestMethod]
        public void NotEqualsTests()
        {
            SObject result = null;
            var processor = ScriptProcessorFactory.GetNew();

            result = processor.Run("2 != 3");

            Assert.IsTrue(result is SBool);
            Assert.AreEqual(((SBool)result).Value, true);

            result = processor.Run("2 !== 3");

            Assert.IsTrue(result is SBool);
            Assert.AreEqual(((SBool)result).Value, true);
        }

        [TestMethod]
        public void LogicalAndTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("true && false");

            Assert.IsTrue(result is SBool);
            Assert.AreEqual(((SBool)result).Value, false);
        }

        [TestMethod]
        public void LogicalOrTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("true || false");

            Assert.IsTrue(result is SBool);
            Assert.AreEqual(((SBool)result).Value, true);
        }
    }
}
