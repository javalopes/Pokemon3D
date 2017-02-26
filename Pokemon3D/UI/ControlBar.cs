using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Common;
using Pokemon3D.Common.Localization;
using Pokemon3D.Rendering.Shapes;
using Pokemon3D.Rendering.UI;
using static Pokemon3D.GameCore.GameProvider;

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
            var bounds = GameInstance.GetService<Window>().ScreenBounds;

            GameInstance.GetService<ShapeRenderer>().DrawRectangle(0, bounds.Height - 64, bounds.Width, 64, Color.White);
            base.Draw(spriteBatch);
        }
    }
}
