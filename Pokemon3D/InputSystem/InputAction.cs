namespace Pokemon3D.InputSystem
{
    public abstract class InputAction
    {
        public string Name { get; protected set; }

        public abstract bool IsPressed();

        public abstract bool IsPressedOnce();
    }
}