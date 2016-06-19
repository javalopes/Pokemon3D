using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.UI
{
    public abstract class UiElementContainer : UiElement
    {
        protected List<UiElement> Elements { get; }

        protected UiElementContainer() : base(null, null)
        {
            Elements = new List<UiElement>();
        }

        public void AddElement(UiElement element)
        {
            Elements.Add(element);
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            Elements.ForEach(e => e.Update(time));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Elements.ForEach(e => e.Draw(spriteBatch));
        }

        public override void Show()
        {
            base.Show();
            Elements.ForEach(e => e.Show());
        }

        public override void Hide()
        {
            base.Hide();
            Elements.ForEach(e => e.Hide());
        }
    }
}