using System;

namespace Assets.Editor.Data
{
    [Serializable]
    public class EntityFieldPositionModel
    {
        public Vector3Model Position;

        public Vector3Model Size;

        public bool Fill;

        public Vector3Model Steps;

        public Vector3Model Rotation;

        public bool CardinalRotation;

        public Vector3Model Scale;
    }
}