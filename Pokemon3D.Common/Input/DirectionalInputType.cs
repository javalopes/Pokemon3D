using System;

namespace Pokemon3D.Common.Input
{
    [Flags]
    public enum DirectionalInputTypes
    {
        /// <summary>
        /// The scroll wheel of the <see cref="Microsoft.Xna.Framework.Input.Mouse"/>.
        /// </summary>
        ScrollWheel = 1 << 0,
        /// <summary>
        /// The DPad of the <see cref="Microsoft.Xna.Framework.Input.GamePad"/>.
        /// </summary>
        DPad = 1 << 1,
        /// <summary>
        /// The left thumbstick of the <see cref="Microsoft.Xna.Framework.Input.GamePad"/>.
        /// </summary>
        LeftThumbstick = 1 << 2,
        /// <summary>
        /// The right thumbstick of the <see cref="Microsoft.Xna.Framework.Input.GamePad"/>.
        /// </summary>
        RightThumbstick = 1 << 3,
        /// <summary>
        /// The WASD keys of the <see cref="Microsoft.Xna.Framework.Input.Keyboard"/>.
        /// </summary>
        WASD = 1 << 4,
        /// <summary>
        /// The arrow keys of the <see cref="Microsoft.Xna.Framework.Input.Keyboard"/>.
        /// </summary>
        ArrowKeys = 1 << 5,
        All = ScrollWheel | DPad | LeftThumbstick | RightThumbstick | WASD | ArrowKeys
    }
}
