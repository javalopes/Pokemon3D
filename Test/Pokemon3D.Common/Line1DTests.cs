using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pokemon3D.Common;

namespace Test.Pokemon3D.Common
{
    [TestClass]
    public class Line1DTests
    {
        [TestMethod]
        public void LineOverlapCalculatedCorrectly1()
        {
            var line1 = new Line1D(0, 10);
            var line2 = new Line1D(80, 90);
            var overlap = line1.OverlapSize(line2);

            Assert.AreEqual(0, overlap);
        }

        [TestMethod]
        public void LineOverlapCalculatedCorrectly2()
        {
            var line1 = new Line1D(0, 50);
            var line2 = new Line1D(40, 90);
            var overlap = line1.OverlapSize(line2);

            Assert.AreEqual(10, overlap);
        }

        [TestMethod]
        public void LineOverlapCalculatedCorrectly3()
        {
            var line1 = new Line1D(0, 50);
            var line2 = new Line1D(40, 45);
            var overlap = line1.OverlapSize(line2);

            Assert.AreEqual(5, overlap);
        }

        [TestMethod]
        public void LineOverlapCalculatedCorrectly4()
        {
            var line1 = new Line1D(0, 100);
            var line2 = new Line1D(0, 20);
            var overlap = line1.OverlapSize(line2);

            Assert.AreEqual(20, overlap);
        }


        [TestMethod]
        public void LineOverlapCalculatedCorrectly5()
        {
            var line1 = new Line1D(0, 10);
            var line2 = new Line1D(80, 90);
            var overlap = line2.OverlapSize(line1);

            Assert.AreEqual(0, overlap);
        }

        [TestMethod]
        public void LineOverlapCalculatedCorrectly6()
        {
            var line1 = new Line1D(0, 50);
            var line2 = new Line1D(40, 90);
            var overlap = line2.OverlapSize(line1);

            Assert.AreEqual(10, overlap);
        }

        [TestMethod]
        public void LineOverlapCalculatedCorrectly7()
        {
            var line1 = new Line1D(0, 50);
            var line2 = new Line1D(40, 45);
            var overlap = line2.OverlapSize(line1);

            Assert.AreEqual(5, overlap);
        }

        [TestMethod]
        public void LineOverlapCalculatedCorrectly8()
        {
            var line1 = new Line1D(0, 100);
            var line2 = new Line1D(0, 20);
            var overlap = line2.OverlapSize(line1);

            Assert.AreEqual(20, overlap);
        }
    }
}
