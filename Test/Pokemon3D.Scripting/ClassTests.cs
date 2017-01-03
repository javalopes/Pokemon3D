
using NUnit.Framework;
using Pokemon3D.Scripting.Types;

namespace Test.Pokemon3D.Scripting
{
    [TestFixture]
    public class ClassTests
    {
        [Test]
        public void InstanciationTests()
        {
            var result = ScriptProcessorFactory.Run("class testClass { } var test = new testClass(); test;");

            Assert.That(result, Is.InstanceOf<SProtoObject>());
            Assert.That(((SProtoObject)result).Prototype.Name, Is.EqualTo("testClass"));
        }

        [Test]
        public void VariableDeclarationTests()
        {
            var result = ScriptProcessorFactory.Run("class testClass { var member = 3 } var test = new testClass(); test.member;");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(3));
        }

        [Test]
        public void ConstructorTests()
        {
            var result = ScriptProcessorFactory.Run("class testClass { var member = 0; function constructor() { this.member = 3; } } var test = new testClass(); test.member;");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(3));
        }

        [Test]
        public void CustomFunctionTests()
        {
            var result = ScriptProcessorFactory.Run("class testClass { var member = 0; function customFunction() { this.member = 3; } } var test = new testClass(); test.customFunction(); test.member;");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(3));
        }

        [Test]
        public void ReadonlyVarTests()
        {
            var result = ScriptProcessorFactory.Run("class testClass { var readonly member = 0; } var test = new testClass(); test.member = 3; test.member;");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(0));
        }

        [Test]
        public void StaticVarTests()
        {
            var result = ScriptProcessorFactory.Run("class testClass { var static member = 0; } testClass.member = 3; testClass.member;");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(3));
        }

        [Test]
        public void StaticFunctionTests()
        {
            var result = ScriptProcessorFactory.Run("class testClass { var static member = 0; function static testFunction() { member = 3; } } testClass.testFunction(); testClass.member;");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(3));
        }

        [Test]
        public void PropertyTests()
        {
            var result = ScriptProcessorFactory.Run("class testClass { var member = 0; function property get Member() { return member; } function property set Member(value) { member = value; } } var test = new testClass(); test.Member = 3; test.Member;");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(3));
        }

        [Test]
        public void IndexerTests()
        {
            var result = ScriptProcessorFactory.Run("class testClass { var member = 0; function indexer get indexerGet(accessor) { return this.member * accessor; } function indexer set indexerSet(accessor, value) { this.member = value / accessor; } } var test = new testClass(); test[3] = 9; test[3];");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(9));
        }

        [Test]
        public void AbstractTests()
        {
            var result = ScriptProcessorFactory.Run("class abstract baseClass { } var test = new baseClass();");

            // cannot instanciate abstract class:
            Assert.That(result, Is.InstanceOf<SError>());
        }

        [Test]
        public void ExtendsTests()
        {
            var result = ScriptProcessorFactory.Run("class abstract baseClass { var member = 3; function baseFunction() { this.member += 2; } } class extendingClass extends baseClass { function extendsFunction() { baseFunction(); } } var test = new extendingClass(); test.extendsFunction(); test.member;");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(5));
        }
    }
}
