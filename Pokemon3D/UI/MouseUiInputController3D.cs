using Microsoft.Xna.Framework;
using Pokemon3D.Entities.System;
using Pokemon3D.Rendering;

namespace Pokemon3D.UI
{
    internal class MouseUiInputController3D : MouseUiInputController
    {
        private readonly Camera _camera;
        private readonly Entity _referringEntity;
        private readonly int _projectedWidth;
        private readonly int _projectedHeight;

        public MouseUiInputController3D(Camera camera, Entity referringEntity, int projectedWidth, int projectedHeight)
        {
            _camera = camera;
            _referringEntity = referringEntity;
            _projectedWidth = projectedWidth;
            _projectedHeight = projectedHeight;
        }

        private static Point ProjectRayOnQuadFor2D(Ray worldRay, Vector3 quadPosition, Vector3 quadNormal, Vector3 quadRight, Vector3 quadDown, Vector2 quadSizeWorld, int quadProjectedWidth, int quadProjectedHeight)
        {
            var plane = new Plane(quadNormal, Vector3.Dot(-quadPosition, quadNormal));

            var hitPoint = worldRay.Intersects(plane);

            if (hitPoint.HasValue)
            {
                var targetPoint = worldRay.Position + worldRay.Direction * hitPoint.Value;

                quadRight.Normalize();
                quadDown.Normalize();

                var rightAxis = quadRight*quadSizeWorld.X;
                var downAxis = quadDown*quadSizeWorld.Y;

                var originPoint = quadPosition - quadRight * quadSizeWorld.X*0.5f - quadDown * quadSizeWorld.Y*0.5f;

                var x = Vector3.Dot((targetPoint - originPoint), rightAxis) / Vector3.Dot(rightAxis, rightAxis);
                var y = Vector3.Dot((targetPoint - originPoint), downAxis) / Vector3.Dot(downAxis, downAxis); ;

                return new Point((int)(x * quadProjectedWidth), (int)(y * quadProjectedHeight));
            }

            return new Point(int.MinValue, int.MinValue);
        }

        protected override Point GetCurrentMousePosition()
        {
            var ray = _camera.GetScreenRay(_inputSystem.MouseHandler.X, _inputSystem.MouseHandler.Y);
            var quadPosition = _referringEntity.GlobalPosition;
            var quadNormal = -_referringEntity.Forward;
            var quadSize = new Vector2(_referringEntity.Scale.X, _referringEntity.Scale.Y);

            return ProjectRayOnQuadFor2D(ray, quadPosition, quadNormal, _referringEntity.Right, -_referringEntity.Up, quadSize, _projectedWidth, _projectedHeight);
        }
    }
}