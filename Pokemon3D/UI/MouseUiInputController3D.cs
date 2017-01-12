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

        protected override Point GetCurrentMousePosition()
        {
            var normal = -_referringEntity.Forward;
            var position = _referringEntity.GlobalPosition;
            var d = Vector3.Dot(-position,normal);
            var plane = new Plane(normal, d);

            var ray = _camera.GetScreenRay(_inputSystem.MouseHandler.X, _inputSystem.MouseHandler.Y);

            var hitPoint = ray.Intersects(plane);

            if (hitPoint.HasValue)
            {
                var targetPoint = ray.Position + ray.Direction*hitPoint.Value;

                var centerPlaneToTarget = targetPoint - position;
                centerPlaneToTarget.Y = -centerPlaneToTarget.Y;
                
                var distanceFromTopLeft = new Vector3(_referringEntity.Scale.X*0.5f, _referringEntity.Scale.Y * 0.5f, 0.0f) + centerPlaneToTarget;

                var x = distanceFromTopLeft.X / _referringEntity.Scale.X * _projectedWidth;
                var y = -distanceFromTopLeft.Y / _referringEntity.Scale.Y * _projectedHeight;

                return new Point((int)x,(int)y);
            }

            return new Point(int.MinValue, int.MinValue);

        }
    }
}