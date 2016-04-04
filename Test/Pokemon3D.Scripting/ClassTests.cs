using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pokemon3D.Scripting.Types;

namespace Test.Pokemon3D.Scripting
{
    [TestClass]
    public class ClassTests
    {
        [TestMethod]
        public void InstanciationTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("class testClass { } var test = new testClass(); test;");

            Assert.IsTrue(result is SProtoObject);
            var protoObj = (SProtoObject)result;
            Assert.AreEqual(protoObj.Prototype.Name, "testClass");
        }

        [TestMethod]
        public void VariableDeclarationTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("class testClass { var member = 3 } var test = new testClass(); test.member;");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 3);
        }

        [TestMethod]
        public void ConstructorTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("class testClass { var member = 0; function constructor() { this.member = 3; } } var test = new testClass(); test.member;");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 3);
        }

        [TestMethod]
        public void CustomFunctionTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("class testClass { var member = 0; function customFunction() { this.member = 3; } } var test = new testClass(); test.customFunction(); test.member;");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 3);
        }

        [TestMethod]
        public void ReadonlyVarTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("class testClass { var readonly member = 0; } var test = new testClass(); test.member = 3; test.member;");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 0);
        }

        [TestMethod]
        public void StaticVarTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("class testClass { var static member = 0; } testClass.member = 3; testClass.member;");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 3);
        }

        [TestMethod]
        public void StaticFunctionTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("class testClass { var static member = 0; function static testFunction() { member = 3; } } testClass.testFunction(); testClass.member;");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 3);
        }

        [TestMethod]
        public void PropertyTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("class testClass { var member = 0; function property get Member() { return member; } function property set Member(value) { member = value; } } var test = new testClass(); test.Member = 3; test.Member;");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 3);
        }

        [TestMethod]
        public void IndexerTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("class testClass { var member = 0; function indexer get indexerGet(accessor) { return this.member * accessor; } function indexer set indexerSet(accessor, value) { this.member = value / accessor; } } var test = new testClass(); test[3] = 9; test[3];");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 9);
        }

        [TestMethod]
        public void AbstractTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("class abstract baseClass { } var test = new baseClass();");

            // cannot instanciate abstract class:
            Assert.IsTrue(result is SError);
        }

        [TestMethod]
        public void ExtendsTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("class abstract baseClass { var member = 3; function baseFunction() { this.member += 2; } } class extendingClass extends baseClass { function extendsFunction() { baseFunction(); } } var test = new extendingClass(); test.extendsFunction(); test.member;");

            Assert.IsTrue(result is SNumber);
            Assert.AreEqual(((SNumber)result).Value, 5);
        }
    }
}
