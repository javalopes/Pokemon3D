using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Common.Localization;
using Pokemon3D.Common.Shapes;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering.UI;

namespace Pokemon3D.UI
{
    /// <summary>
    /// Draws a bar to the bottom part of the screen that can display button functions for the current screen.
    /// </summary>
    internal class ControlBar : UiCompoundElement
    {
        private int _currentIndex;

        public void AddEntry(LocalizedValue text, Buttons gamePadButton, Keys keyboardKey)
        {
            AddChildElement(new BarEntry
            {
                Text = text,
                GamePadButton = gamePadButton,
                KeyboardKey = keyboardKey,
                Index = _currentIndex++
            });
        }

        public override bool IsInteractable => false;

        public override void Draw(SpriteBatch spriteBatch)
        {
            GameProvider.GameInstance.GetService<ShapeRenderer>().DrawRectangle(0, GameProvider.GameInstance.ScreenBounds.Height - 64, GameProvider.GameInstance.ScreenBounds.Width, 64, Color.White);
            base.Draw(spriteBatch);
        }
    }
}
