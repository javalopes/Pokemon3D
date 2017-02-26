using System;
using Microsoft.Xna.Framework;

namespace Pokemon3D.Common
{
    public class Window
    {
        private readonly GameWindow _window;

        public Window(GameWindow window)
        {
            _window = window;
            ScreenBounds = _window.ClientBounds;
            _window.ClientSizeChanged += OnClientSizeChanged;
        }

        private void OnClientSizeChanged(object sender, EventArgs e)
        {
            var old = ScreenBounds;
            ScreenBounds = new Rectangle(0, 0, _window.ClientBounds.Width, _window.ClientBounds.Height);
            if (WindowSizeChanged != null && old != _window.ClientBounds)
            {
                WindowSizeChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler WindowSizeChanged;

        public Rectangle ScreenBounds { get; private set; }
    }
}
