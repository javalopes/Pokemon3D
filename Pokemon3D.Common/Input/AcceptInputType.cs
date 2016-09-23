using System;

namespace Pokemon3D.Common.Input
{
    [Flags]
    public enum AcceptInputTypes
    {
        /// <summary>
        /// The enter key on the <see cref="Microsoft.Xna.Framework.Input.Keyboard"/>.
        /// </summary>
        EnterKey = 1 << 0,
        /// <summary>
        /// The spacebar key on the <see cref="Microsoft.Xna.Framework.Input.Keyboard"/>.
        /// </summary>
        SpacebarKey = 1 << 1,
        /// <summary>
        /// The left button on the <see cref="Microsoft.Xna.Framework.Input.Mouse"/>.
        /// </summary>
        LeftClick = 1 << 2,
        /// <summary>
        /// The A button on the <see cref="Microsoft.Xna.Framework.Input.GamePad"/>.
        /// </summary>
        AButton = 1 << 3,
        All = EnterKey | SpacebarKey | LeftClick | AButton,
        Buttons = EnterKey | SpacebarKey | AButton
    }
}
