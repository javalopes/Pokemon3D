using NUnit.Framework;
using Pokemon3D.Scripting;
using Pokemon3D.Scripting.Adapters;
using Pokemon3D.Scripting.Types;

namespace Test.Pokemon3D.Scripting.Adapters
{
    [TestFixture]
    public class ScriptInAdapterTests
    {
        [Test]
        public void StringTranslateTest()
        {
            const string testString = "this is a test string!";
            var processor = ScriptProcessorFactory.GetNew();

            var obj = ScriptInAdapter.Translate(processor, testString);

            Assert.That(obj, Is.InstanceOf<SString>());
            Assert.That(testString, Is.EqualTo(((SString)obj).Value));
        }

        [Test]
        public void NumericTranslateTest()
        {
            const double testDouble = -72.64;
            var processor = ScriptProcessorFactory.GetNew();

            var obj = ScriptInAdapter.Translate(processor, testDouble);

            Assert.That(obj, Is.InstanceOf<SNumber>());
            Assert.That(testDouble, Is.EqualTo(((SNumber)obj).Value));
        }

        [Test]
        public void NullTranslateTest()
        {
            var processor = ScriptProcessorFactory.GetNew();

            var obj = ScriptInAdapter.Translate(processor, null);

            Assert.That(obj, Is.InstanceOf<SNull>(), "The type of obj is not SNull, but instead " + obj.GetType().Name);
        }
        
        [Test]
        public void UndefinedTranslateTest()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var undefined = processor.Undefined;

            var obj = ScriptInAdapter.Translate(processor, undefined);
            
            Assert.That(obj, Is.InstanceOf<SUndefined>());

        }

        [Test]
        public void BooleanTranslateTest()
        {
            const bool testBool = true;
            var processor = ScriptProcessorFactory.GetNew();

            var obj = ScriptInAdapter.Translate(processor, testBool);

            Assert.That(obj, Is.InstanceOf<SBool>());
            Assert.That(testBool, Is.EqualTo(((SBool)obj).Value));
        }

        [Test]
        public void ArrayTranslateTest()
        {
            var testArray = new[] { "item1", "item2", "item3" };
            var processor = ScriptProcessorFactory.GetNew();

            var obj = ScriptInAdapter.Translate(processor, testArray);

            Assert.That(obj, Is.InstanceOf<SArray>());
            Assert.That(testArray.Length, Is.EqualTo(((SArray)obj).ArrayMembers.Length));
        }

        [Test]
        public void ObjectTranslateTest()
        {
            var processor = new ScriptProcessor();
            ScriptContextManipulator.AddPrototype(processor, typeof(Pokemon));

            processor.Run("var p = new Pokemon(); p.SetName(\"Pika\");");

            var objp = ScriptContextManipulator.GetVariableTranslated(processor, "p");
            
            Assert.That(objp, Is.InstanceOf<Pokemon>());

            var p = (Pokemon)objp;

            Assert.That("Pikachu", Is.EqualTo(p.OriginalName));
            Assert.That("Pika", Is.EqualTo(p.Name));
        }

        // Test class to create instances of.
        private class Pokemon
        {
            [ScriptVariable]
            public string Name = "Pikachu";

            [ScriptVariable]
            public string OriginalName = "";

            [ScriptFunction(ScriptFunctionType.Standard)]
#pragma warning disable 169
            public string SetName = "function(name) { if (this.OriginalName == \"\") { this.OriginalName = this.Name; } this.Name = name; }";
#pragma warning restore 169
        }
    }
}