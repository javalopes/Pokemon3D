using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.UI
{
    public abstract class UiElementContainer : UiBaseElement
    {
        protected List<UiElement> Elements { get; }

        protected UiElementContainer()
        {
            Elements = new List<UiElement>();
        }

        public void AddElement(UiElement element)
        {
            Elements.Add(element);
        }

        public override void Update(GameTime time)
        {
            Elements.ForEach(e => e.Update(time));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Elements.ForEach(e =>
            {
                if (e.State != UiState.Inactive) e.Draw(spriteBatch);
            });
        }

        public override bool IsAnimating
        {
            get { return Elements.Any(e => e.IsAnimating); }
        }

        public override void Show()
        {
            Elements.ForEach(e => e.Show());
        }

        public override void Hide()
        {
            Elements.ForEach(e => e.Hide());
        }
    }
}