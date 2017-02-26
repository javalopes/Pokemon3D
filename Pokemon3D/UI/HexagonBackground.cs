using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Content;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering.UI;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.UI
{
    /// <summary>
    /// Displays a half-visible background filled with hexagons.
    /// </summary>
    internal class HexagonBackground : UiCompoundElement
    {
        private readonly Texture2D _hexagonTexture;

        public HexagonBackground()
        {
            _hexagonTexture = GameInstance.GetService<ContentManager>().Load<Texture2D>(ResourceNames.Textures.UI.Common.Hexagon);
            GenerateHexagons(true);
        }

        private void GenerateHexagons(bool hasAnimation)
        {
            var bounds = GameInstance.GetService<Window>().ScreenBounds;
            ClearChildren();
            for (var x = -1; x < bounds.Width/Hexagon.Width + 1; x++)
            {
                for (var y = -1; y < bounds.Height / Hexagon.HeightHalf + 1; y++)
                {
                    var hexagon = new Hexagon(_hexagonTexture, x, y, hasAnimation);
                    AddChildElement(hexagon);
                }
            }
        }

        public override bool IsInteractable => false;
    }
}
