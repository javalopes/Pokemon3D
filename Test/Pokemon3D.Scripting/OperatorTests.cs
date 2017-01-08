using NUnit.Framework;
using Pokemon3D.Scripting.Types;

namespace Test.Pokemon3D.Scripting
{
    [TestFixture]
    public class OperatorTests
    {
        [Test]
        public void AdditionTests()
        {
            var result = ScriptProcessorFactory.Run("1+4+6");


            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(11));
        }

        [Test]
        public void SubtractionTests()
        {
            var result = ScriptProcessorFactory.Run("12-4-6");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(2));
        }

        [Test]
        public void MultiplicationTests()
        {
            var result = ScriptProcessorFactory.Run("5*3*-4");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(-60));
        }

        [Test]
        public void DivisionTests()
        {
            var result = ScriptProcessorFactory.Run("15/5");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(3));
        }

        [Test]
        public void IncrementTests()
        {
            var result = ScriptProcessorFactory.Run("var a = 13; a++; a;");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(14));

        }

        [Test]
        public void DecrementTests()
        {
            var result = ScriptProcessorFactory.Run("var b = 13; b--; b;");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(12));
        }

        [Test]
        public void PowerTests()
        {
            var result = ScriptProcessorFactory.Run("2**3");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(8));
        }

        [Test]
        public void ModuloTests()
        {
            var result = ScriptProcessorFactory.Run("5%2");

            Assert.That(result, Is.InstanceOf<SNumber>());
            Assert.That(((SNumber)result).Value, Is.EqualTo(1));
        }

        [Test]
        public void SmallerThanTests()
        {
            var processor = ScriptProcessorFactory.GetNew();

            var result = processor.Run("2 < 3");

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.True);

            result = processor.Run("3 < -5");

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.False);

            result = processor.Run("\"test\" < -5");

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.False);
        }

        [Test]
        public void SmallerThanOrEqualsTests()
        {
            var processor = ScriptProcessorFactory.GetNew();

            var result = processor.Run("2 <= 3");

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.True);

            result = processor.Run("3 <= 3");

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.True);

            result = processor.Run("\"test\" <= 0");

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.False);
        }

        [Test]
        public void GreaterThanTests()
        {
            var processor = ScriptProcessorFactory.GetNew();

            var result = processor.Run("3 > 2");

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.True);

            result = processor.Run("1 > -5");

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.True);

            result = processor.Run("\"test\" > -5");

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.False);
        }

        [Test]
        public void GreaterThanOrEqualsTests()
        {
            var processor = ScriptProcessorFactory.GetNew();

            var result = processor.Run("3 >= 2");

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.True);

            result = processor.Run("3 >= 3");

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.True);

            result = processor.Run("\"test\" >= 0");

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.False);
        }

        [Test]
        public void EqualsTests()
        {
            var processor = ScriptProcessorFactory.GetNew();

            var result = processor.Run("2 == 2");

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.True);

            result = processor.Run("2 === 2");

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.True);
        }

        [Test]
        public void NotEqualsTests()
        {
            var processor = ScriptProcessorFactory.GetNew();

            var result = processor.Run("2 != 3");

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.True);

            result = processor.Run("2 !== 3");

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.True);
        }

        [Test]
        public void LogicalAndTests()
        {
            var result = ScriptProcessorFactory.Run("true && false");

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.False);
        }

        [Test]
        public void LogicalOrTests()
        {
            var result = ScriptProcessorFactory.Run("true || false");

            Assert.That(result, Is.InstanceOf<SBool>());
            Assert.That(((SBool)result).Value, Is.True);
        }
    }
}
