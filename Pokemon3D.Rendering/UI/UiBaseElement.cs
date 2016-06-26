using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.UI
{
    public abstract class UiBaseElement
    {
        public Color Color { get; set; }
        public Vector2 Offset { get; set; }
        public float Alpha { get; set; }
        public UiState State { get; protected set; }
        public abstract bool IsAnimating { get; }

        protected UiBaseElement()
        {
            Alpha = 1.0f;
            Color = Color.White;
            Offset = Vector2.Zero;
        }

        public abstract void Show();
        public abstract void Hide();
        public abstract void Focus();
        public abstract void Unfocus();
        public abstract void Update(GameTime time);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}