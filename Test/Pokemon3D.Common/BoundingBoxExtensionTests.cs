
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Pokemon3D.Common.Extensions;

namespace Test.Pokemon3D.Common
{
    [TestClass]
    public class BoundingBoxExtensionTests
    {
        [TestMethod]
        public void CheckCollision1()
        {
            var distance = new Vector3(20,0,0);
            var boundingBox1 = new BoundingBox(new Vector3(-5), new Vector3(5));
            var boundingBox2 = new BoundingBox(new Vector3(-5)+ distance, new Vector3(5)+ distance);

            var collisionResult = boundingBox1.CollidesWithSat(boundingBox2);

            Assert.IsNotNull(collisionResult);
            Assert.IsFalse(collisionResult.Collides);
        }

        [TestMethod]
        public void CheckCollision2()
        {
            var distance = new Vector3(5, 0, 0);
            var boundingBox1 = new BoundingBox(new Vector3(-5), new Vector3(5));
            var boundingBox2 = new BoundingBox(new Vector3(-5) + distance, new Vector3(5) + distance);

            var collisionResult = boundingBox1.CollidesWithSat(boundingBox2);

            Assert.IsNotNull(collisionResult);
            Assert.IsTrue(collisionResult.Collides);
        }
    }
}
