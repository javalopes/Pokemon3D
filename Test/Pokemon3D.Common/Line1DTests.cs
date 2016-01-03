
using NUnit.Framework;
using Pokemon3D.Common;

namespace Test.Pokemon3D.Common
{
    [TestFixture]
    public class Line1DTests
    {
        [TestCase(0,10,80,90, 0)]
        [TestCase(0, 50, 40, 90, 10)]
        [TestCase(0,50,40,45, 5)]
        [TestCase(0,100,0,20, 20)]
        [TestCase( 80, 90, 0, 10, 0)]
        [TestCase( 40, 90, 0, 50, 10)]
        [TestCase( 40, 45, 0, 50, 5)]
        [TestCase( 0, 20, 0, 100, 20)]
        public void LineOverlapCalculatedCorrectly1(float min1, float max1, float min2, float max2, float overlap)
        {
            var line1 = new Line1D(min1, max1);
            var line2 = new Line1D(min2, max2);
            var calculatedOverlap = line1.OverlapSize(line2);

            Assert.That(calculatedOverlap, Is.EqualTo(overlap));
        }
    }
}
