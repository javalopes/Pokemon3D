using System;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering.UI;

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
            GameProvider.GameInstance.WindowSizeChanged += HandleWindowSizeChanged;
            _hexagonTexture = GameProvider.GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.UI.Common.Hexagon);
            GenerateHexagons(true);
        }

        private void HandleWindowSizeChanged(object sender, EventArgs e)
        {
            GenerateHexagons(false);
        }

        private void GenerateHexagons(bool hasAnimation)
        {
            ClearChildren();
            for (var x = -1; x < GameProvider.GameInstance.ScreenBounds.Width/Hexagon.Width + 1; x++)
            {
                for (var y = -1; y < GameProvider.GameInstance.ScreenBounds.Height / Hexagon.HeightHalf + 1; y++)
                {
                    var hexagon = new Hexagon(_hexagonTexture, x, y, hasAnimation);
                    AddChildElement(hexagon);
                }
            }
        }

        public override bool IsInteractable => false;
    }
}
