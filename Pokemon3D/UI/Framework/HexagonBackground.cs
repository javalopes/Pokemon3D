using Pokemon3D.GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Pokemon3D.Rendering.UI;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.UI.Framework
{
    /// <summary>
    /// Displays a half-visible background filled with hexagons.
    /// </summary>
    class HexagonBackground : UiCompoundElement
    {
        private readonly Texture2D _hexagonTexture;

        public HexagonBackground()
        {
            GameInstance.WindowSizeChanged += HandleWindowSizeChanged;
            _hexagonTexture = GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.UI.Common.Hexagon);
            GenerateHexagons(true);
        }

        private void HandleWindowSizeChanged(object sender, EventArgs e)
        {
            GenerateHexagons(false);
        }

        private void GenerateHexagons(bool hasAnimation)
        {
            ClearChildren();
            for (var x = -1; x < GameInstance.ScreenBounds.Width/Hexagon.Width + 1; x++)
            {
                for (var y = -1; y < GameInstance.ScreenBounds.Height / Hexagon.HeightHalf + 1; y++)
                {
                    var hexagon = new Hexagon(_hexagonTexture, x, y, hasAnimation);
                    AddChildElement(hexagon);
                }
            }
        }

        public override bool IsInteractable => false;
    }
}
