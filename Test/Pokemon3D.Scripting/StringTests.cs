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

            Assert.IsTrue(result is SString);
            Assert.AreEqual("{Hello}, {{World}}!", ((SString)result).Value);
        }
    }
}
