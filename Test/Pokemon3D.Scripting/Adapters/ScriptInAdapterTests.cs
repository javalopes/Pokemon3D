﻿using NUnit.Framework;
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
            string testString = "this is a test string!";
            var processor = ScriptProcessorFactory.GetNew();

            var obj = ScriptInAdapter.Translate(processor, testString);

            Assert.IsTrue(obj.GetType() == typeof(SString));
            Assert.AreEqual(testString, ((SString)obj).Value);
        }

        [Test]
        public void NumericTranslateTest()
        {
            double testDouble = -72.64;
            var processor = ScriptProcessorFactory.GetNew();

            var obj = ScriptInAdapter.Translate(processor, testDouble);

            Assert.IsTrue(obj.GetType() == typeof(SNumber));
            Assert.AreEqual(testDouble, ((SNumber)obj).Value);
        }

        [Test]
        public void NullTranslateTest()
        {
            object nullObj = null;
            var processor = ScriptProcessorFactory.GetNew();

            var obj = ScriptInAdapter.Translate(processor, nullObj);

            Assert.IsTrue(obj.GetType() == typeof(SNull), "The type of obj is not SNull, but instead " + obj.GetType().Name);
        }
        
        [Test]
        public void UndefinedTranslateTest()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var undefined = processor.Undefined;

            var obj = ScriptInAdapter.Translate(processor, undefined);

            Assert.IsTrue(obj.GetType() == typeof(SUndefined));
        }

        [Test]
        public void BooleanTranslateTest()
        {
            bool testBool = true;
            var processor = ScriptProcessorFactory.GetNew();

            var obj = ScriptInAdapter.Translate(processor, testBool);

            Assert.IsTrue(obj.GetType() == typeof(SBool));
            Assert.AreEqual(testBool, ((SBool)obj).Value);
        }

        [Test]
        public void ArrayTranslateTest()
        {
            string[] testArray = new string[] { "item1", "item2", "item3" };
            var processor = ScriptProcessorFactory.GetNew();

            var obj = ScriptInAdapter.Translate(processor, testArray);

            Assert.IsTrue(obj.GetType() == typeof(SArray));

            var arr = (SArray)obj;

            Assert.AreEqual(testArray.Length, arr.ArrayMembers.Length);
        }

        [Test]
        public void ObjectTranslateTest()
        {
            var processor = new ScriptProcessor();
            ScriptContextManipulator.AddPrototype(processor, typeof(Pokemon));

            processor.Run("var p = new Pokemon(); p.SetName(\"Pika\");");

            object objp = ScriptContextManipulator.GetVariableTranslated(processor, "p");

            Assert.IsTrue(objp.GetType() == typeof(Pokemon));

            Pokemon p = (Pokemon)objp;

            Assert.AreEqual("Pikachu", p.OriginalName);
            Assert.AreEqual("Pika", p.Name);
        }

        // Test class to create instances of.
        private class Pokemon
        {
            [ScriptVariable]
            public string Name = "Pikachu";
            [ScriptVariable]
            public string OriginalName = "";

            [ScriptFunction(ScriptFunctionType.Standard)]
            public string SetName = "function(name) { if (this.OriginalName == \"\") { this.OriginalName = this.Name; } this.Name = name; }";
        }
    }
}