using System;

namespace Pokemon3D.Common.Input
{
    [Flags]
    public enum DismissInputTypes
    {
        /// <summary>
        /// The E key on the <see cref="Microsoft.Xna.Framework.Input.Keyboard"/>.
        /// </summary>
        EKey = 1 << 0,
        /// <summary>
        /// The escape key on the <see cref="Microsoft.Xna.Framework.Input.Keyboard"/>.
        /// </summary>
        EscapeKey = 1 << 1,
        /// <summary>
        /// The right button on the <see cref="Microsoft.Xna.Framework.Input.Mouse"/>.
        /// </summary>
        RightClick = 1 << 2,
        /// <summary>
        /// The B button on the <see cref="Microsoft.Xna.Framework.Input.GamePad"/>.
        /// </summary>
        BButton = 1 << 3,
        All = EKey | EscapeKey | RightClick | BButton,
        Buttons = EKey | EscapeKey | BButton
    }
}
