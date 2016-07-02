using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace Pokemon3D.Rendering.UI
{
    public abstract class UiElementContainerBase : UiElementContainer
    {
        private readonly List<UiElement> _elements;

        public ReadOnlyCollection<UiElement> UiElements { get; private set; }

        protected UiElementContainerBase()
        {
            _elements = new List<UiElement>();
            UiElements = _elements.AsReadOnly();
        }
        
        public void AddElement(UiElement uiElement)
        {
            _elements.Add(uiElement);
        }

        public void AddRange(IEnumerable<UiElement> uiElements)
        {
            _elements.AddRange(uiElements);
        }

        public abstract void Update(GameTime gameTime);
    }
}