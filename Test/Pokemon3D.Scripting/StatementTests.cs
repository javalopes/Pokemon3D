using NUnit.Framework;
using Pokemon3D.Scripting.Types;

namespace Test.Pokemon3D.Scripting
{
    [TestFixture]
    public class StatementTests
    {
        [Test]
        public void IfTests()
        {
            var result = ScriptProcessorFactory.Run("var a = 0; if (a == 0) { a = 5; } a;");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(5));
        }

        [Test]
        public void IfElseTests()
        {
            var result = ScriptProcessorFactory.Run("var a = 0; if (a > 0) { a = 5; } else { a = 10; } a;");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(10));
        }

        [Test]
        public void ForLoopTests()
        {
            var result = ScriptProcessorFactory.Run("var a = 0; for (var i = 0; i < 10; i++) { a++; } a;");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(10));
        }

        [Test]
        public void WhileLoopTests()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var result = processor.Run("var a = 0; while (a < 10) { a++; } a;");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(10));
        }

        [Test]
        public void ContinueTests()
        {
            var result = ScriptProcessorFactory.Run("var a = 0; for (var i = 0; i < 10; i++) { if (i % 2 == 0) { a++; } } a;");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(5));
        }

        [Test]
        public void BreakTests()
        {
            var result = ScriptProcessorFactory.Run("var a = 0; for (var i = 0; i < 10; i++) { if (i > 5) { break; } a++; } a;");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(6));
        }
    }
}
