using System;

namespace Pokemon3D.Common.Input
{
    [Flags]
    public enum AcceptInputTypes
    {
        /// <summary>
        /// The enter key on the <see cref="Microsoft.Xna.Framework.Input.Keyboard"/>.
        /// </summary>
        EnterKey = 1,
        /// <summary>
        /// The spacebar key on the <see cref="Microsoft.Xna.Framework.Input.Keyboard"/>.
        /// </summary>
        SpacebarKey = 2,
        /// <summary>
        /// The left button on the <see cref="Microsoft.Xna.Framework.Input.Mouse"/>.
        /// </summary>
        LeftClick = 4,
        /// <summary>
        /// The A button on the <see cref="Microsoft.Xna.Framework.Input.GamePad"/>.
        /// </summary>
        AButton = 8,
        All = EnterKey | SpacebarKey | LeftClick | AButton,
        Buttons = EnterKey | SpacebarKey | AButton
    }
}
