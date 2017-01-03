using NUnit.Framework;
using Pokemon3D.Scripting.Types;

namespace Test.Pokemon3D.Scripting
{
    [TestFixture]
    public class StringTests
    {
        [Test]
        public void InterpolationTests()
        {
            var result = ScriptProcessorFactory.Run("var insert = \"{{World}}\"; var outer = $\"{{Hello}}, {insert}!\"; outer;");

            Assert.That(result , Is.InstanceOf<SString>());
            Assert.That(((SString)result).Value, Is.EqualTo("{Hello}, {{World}}!"));
        }
    }
}
