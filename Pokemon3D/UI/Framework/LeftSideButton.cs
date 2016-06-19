﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Input;
using System;
using Pokemon3D.Rendering.UI;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.UI.Framework
{
    class LeftSideButton : UiElement
    {
        private readonly SpriteFont _font;
        private Vector2 _position;
        private readonly Action<LeftSideButton> _onClick;

        public string Text { get; set; }

        public LeftSideButton(string text, Vector2 position, Action<LeftSideButton> onClick) : base(GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.UI.Common.Button_Blank))
        {
            _font = GameInstance.Content.Load<SpriteFont>(ResourceNames.Fonts.NormalFont);

            Text = text;
            _position = position;
            var bounds = Bounds;
            bounds.X = (int) position.X;
            bounds.Y = (int) position.Y;
            bounds.Width = 200;
            bounds.Height = 38;
            Bounds = bounds;
            _onClick = onClick;

            HoverAnimation = new UiColorAnimation(0.5f, new Color(255, 255, 255), new Color(100, 193, 238));
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            if (_onClick != null)
            {
                if (GameInstance.InputSystem.Accept(AcceptInputTypes.Buttons) ||
                    Bounds.Contains(GameInstance.InputSystem.Mouse.Position) && GameInstance.InputSystem.Accept(AcceptInputTypes.LeftClick))
                {
                    _onClick(this);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawTexture(spriteBatch);
            spriteBatch.DrawString(_font, Text, new Vector2(_position.X + 24, _position.Y + 5), Color.Black);
        }
    }
}
