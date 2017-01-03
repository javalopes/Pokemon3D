using Pokemon3D.Scripting.Adapters;
using Pokemon3D.Scripting.Types;
using System;
using NUnit.Framework;

namespace Test.Pokemon3D.Scripting.Adapters
{
    [TestFixture]
    public class ScriptOutAdapterTests
    {
        [Test]
        public void SStringTranslateTest()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var testString = processor.CreateString("this is a test string!");

            var obj = ScriptOutAdapter.Translate(testString);

            Assert.That(obj, Is.InstanceOf<string>());
            Assert.That((string)obj, Is.EqualTo(testString.Value));
        }

        [Test]
        public void SNumberTranslateTest()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var testNumber = processor.CreateNumber(-72.65);

            var obj = ScriptOutAdapter.Translate(testNumber);
            
            Assert.That(obj, Is.InstanceOf<double>());
            Assert.That((double)obj, Is.EqualTo(testNumber.Value));
        }

        [Test]
        public void SBoolTranslateTest()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var testBool = processor.CreateBool(true);

            var obj = ScriptOutAdapter.Translate(testBool);

            Assert.That(obj, Is.InstanceOf<bool>());
            Assert.That((bool)obj, Is.EqualTo(testBool.Value));
        }

        [Test]
        public void SNullTranslateTest()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var @null = (SNull)processor.Null;

            var obj = ScriptOutAdapter.Translate(@null);

            Assert.That(obj, Is.Null);
        }

        [Test]
        public void SUndefinedTranslateTest()
        {
            var processor = ScriptProcessorFactory.GetNew();
            var undefined = (SUndefined)processor.Undefined;

            var obj = ScriptOutAdapter.Translate(undefined);

            Assert.That(obj, Is.InstanceOf<SUndefined>());
        }

        [Test]
        public void SArrayTranslateTest()
        {
            var processor = ScriptProcessorFactory.GetNew();

            SObject arrobj = processor.CreateArray(new SObject[]
            {
                processor.CreateBool(true),
                processor.CreateString("test"),
                processor.CreateNumber(-1234.3)
            });
            Assert.That(arrobj, Is.InstanceOf<SArray>());

            var obj = ScriptOutAdapter.Translate(arrobj);

            Assert.That(obj.GetType().IsArray);
            Assert.IsTrue(((Array)obj).Length == 3);
        }
    }
}
