using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Pokemon3D.Rendering.UI;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.UI.Framework
{
    /// <summary>
    /// Draws a bar to the bottom part of the screen that can display button functions for the current screen.
    /// </summary>
    class ControlBar : UiCompoundElement
    {
        private int _currentIndex;

        public void AddEntry(string text, Buttons gamePadButton, Keys keyboardKey)
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
            GameInstance.ShapeRenderer.DrawRectangle(0, GameInstance.ScreenBounds.Height - 64, GameInstance.ScreenBounds.Width, 64, Color.White);
            base.Draw(spriteBatch);
        }
    }
}
