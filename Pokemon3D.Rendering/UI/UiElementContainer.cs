using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace Pokemon3D.Rendering.UI
{
    public interface UiElementContainer
    {
        ReadOnlyCollection<UiElement> UiElements { get; }

        void AddElement(UiElement uiElement);
        void AddRange(IEnumerable<UiElement> uiElements);
        void Update(GameTime gameTime);
    }
}
