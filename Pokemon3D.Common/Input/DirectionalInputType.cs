using System;

namespace Pokemon3D.Common.Input
{
    [Flags]
    public enum DirectionalInputTypes
    {
        /// <summary>
        /// The scroll wheel of the <see cref="Microsoft.Xna.Framework.Input.Mouse"/>.
        /// </summary>
        ScrollWheel = 1,
        /// <summary>
        /// The DPad of the <see cref="Microsoft.Xna.Framework.Input.GamePad"/>.
        /// </summary>
        DPad = 2,
        /// <summary>
        /// The left thumbstick of the <see cref="Microsoft.Xna.Framework.Input.GamePad"/>.
        /// </summary>
        LeftThumbstick = 4,
        /// <summary>
        /// The right thumbstick of the <see cref="Microsoft.Xna.Framework.Input.GamePad"/>.
        /// </summary>
        RightThumbstick = 8,
        /// <summary>
        /// The WASD keys of the <see cref="Microsoft.Xna.Framework.Input.Keyboard"/>.
        /// </summary>
        WASD = 16,
        /// <summary>
        /// The arrow keys of the <see cref="Microsoft.Xna.Framework.Input.Keyboard"/>.
        /// </summary>
        ArrowKeys = 32
    }
}
