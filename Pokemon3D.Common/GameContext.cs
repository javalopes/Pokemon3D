﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Resources;

namespace Pokemon3D.Common
{
    public interface GameContext
    {
        KeyboardEx Keyboard { get; }
        ContentManager Content { get; }
        Rectangle ScreenBounds { get; }
        SpriteBatch SpriteBatch { get; }
        ShapeRenderer ShapeRenderer { get; }
        GraphicsDevice GraphicsDevice { get; }
        Localization.TranslationProvider TranslationProvider { get; }
        string VersionInformation { get; }
        Texture2DProvider Texture2DProvider { get; }
    }
}
