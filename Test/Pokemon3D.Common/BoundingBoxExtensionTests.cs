using Microsoft.Xna.Framework;
using NUnit.Framework;
using Pokemon3D.Common.Extensions;

namespace Test.Pokemon3D.Common
{
    [TestFixture]
    public class BoundingBoxExtensionTests
    {
        [TestCase(20,0,0, false, 0, 0, 0)]
        [TestCase(10, 8, 0, false, 0, 0, 0)]
        [TestCase(-20, 0, 0, false, 0, 0, 0)]
        [TestCase(-10, -8, 0, false, 0, 0, 0)]
        [TestCase(5,0,0, true, 5, 0, 0)]
        [TestCase(9, 0, 0, true, 1, 0, 0)]
        [TestCase(9, 8, 0, true, 1, 0, 0)]
        [TestCase(-5, 0, 0, true, -5, 0, 0)]
        [TestCase(-9, 0, 0, true, -1, 0, 0)]
        [TestCase(-9, -8, 0, true, -1, 0, 0)]
        [TestCase(6, 3, 9, true, 0, 0, 1)]
        public void CheckCollision(float x, float y, float z, bool hasCollision, float separationX, float separationY, float separationZ)
        {
            var distance = new Vector3(x,y,z);
            var boundingBox1 = new BoundingBox(new Vector3(-5), new Vector3(5));
            var boundingBox2 = new BoundingBox(new Vector3(-5)+ distance, new Vector3(5) + distance);

            var collisionResult = boundingBox1.CollidesWithSat(boundingBox2);
            var separation = new Vector3(separationX, separationY, separationZ);

            Assert.That(collisionResult, Is.Not.Null);
            Assert.That(collisionResult.Collides, Is.EqualTo(hasCollision));
            Assert.That(collisionResult.Axis, Is.EqualTo(separation));
        }
    }
}
