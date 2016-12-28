namespace Pokemon3D.Common.Input
{
    public abstract class InputAction
    {
        public string Name { get; protected set; }

        public abstract bool IsPressed();

        public abstract bool IsPressedOnce();
    }
}