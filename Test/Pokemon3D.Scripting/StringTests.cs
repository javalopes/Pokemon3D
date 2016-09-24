using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pokemon3D.Scripting.Types;

namespace Test.Pokemon3D.Scripting
{
    [TestClass]
    public class StringTests
    {
        [TestMethod]
        public void InterpolationTests()
        {
            var result = ScriptProcessorFactory.Run("var insert = \"{{World}}\"; var outer = $\"{{Hello}}, {insert}!\"; outer;");

            Assert.IsTrue(result is SString);
            Assert.AreEqual("{Hello}, {{World}}!", ((SString)result).Value);
        }
    }
}
