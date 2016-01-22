using System;

namespace Pokemon3D.Common.Input
{
    [Flags]
    public enum DismissInputTypes
    {
        /// <summary>
        /// The E key on the <see cref="Microsoft.Xna.Framework.Input.Keyboard"/>.
        /// </summary>
        EKey = 1,
        /// <summary>
        /// The escape key on the <see cref="Microsoft.Xna.Framework.Input.Keyboard"/>.
        /// </summary>
        EscapeKey = 2,
        /// <summary>
        /// The right button on the <see cref="Microsoft.Xna.Framework.Input.Mouse"/>.
        /// </summary>
        RightClick = 4,
        /// <summary>
        /// The B button on the <see cref="Microsoft.Xna.Framework.Input.GamePad"/>.
        /// </summary>
        BButton = 8
    }
}
