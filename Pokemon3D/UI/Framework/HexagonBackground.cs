using Pokemon3D.GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon3D.UI.Framework
{
    /// <summary>
    /// Displays a half-visible background filled with hexagons.
    /// </summary>
    class HexagonBackground : GameObject
    {
        private const int HEXAGON_WIDTH = 26;
        private const int HEXAGON_HEIGHT = 31;
        private const int HEXAGON_HEIGHT_HALF = 15;

        private class Hexagon
        {
            private const int MIN_ALPHA = 100;
            private const int MAX_ALPHA = 200;
            private const float DELAY_VERTICAL_OFFSET_MULTIPLIER = 0.2f;
            private const float ALPHA_FADE_EDGE_VALUE = 10f;
            private const int BLINKING_CHANCE = 40000;

            private int _x, _y;
            private float _delay = 0f;
            private float _originalAlpha;
            private float _targetAlpha;
            private bool _isAnimating = false;

            public float Alpha { get; private set; }

            public bool Visible { get { return _delay == 0f; } }

            public Hexagon(int x, int y, bool hasAnimation)
            {
                _x = x;
                _y = y;
                _targetAlpha = Common.GlobalRandomProvider.Instance.Rnd.Next(MIN_ALPHA, MAX_ALPHA);
                
                if (hasAnimation)
                {
                    _delay = y * DELAY_VERTICAL_OFFSET_MULTIPLIER;
                    Alpha = 0f;
                }
                else
                {
                    _delay = 0f;
                    Alpha = _targetAlpha;
                }

                _originalAlpha = _targetAlpha;
            }

            public void Update()
            {
                if (_delay > 0f)
                {
                    _delay -= 0.1f;
                    if (_delay <= 0f)
                        _delay = 0f;
                }
                else
                {
                    if (Alpha != _targetAlpha)
                    {
                        Alpha = MathHelper.SmoothStep(_targetAlpha, Alpha, 0.7f);
                        if (Math.Abs(Alpha - _targetAlpha) < ALPHA_FADE_EDGE_VALUE)
                        {
                            Alpha = _targetAlpha;
                        }
                    }
                    else
                    {
                        if (_isAnimating)
                        {
                            _isAnimating = false;
                            _targetAlpha = _originalAlpha;
                        }
                        else
                        {
                            if (Common.GlobalRandomProvider.Instance.Rnd.Next(0, BLINKING_CHANCE) == 0)
                            {
                                if (Common.GlobalRandomProvider.Instance.Rnd.Next(0, 2) == 0)
                                    _targetAlpha = 255;
                                else
                                    _targetAlpha = 0;
                                _isAnimating = true;
                            }
                        }
                    }
                }
            }

            public Vector2 GetOffset()
            {
                return new Vector2(_x * HEXAGON_WIDTH, _y * HEXAGON_HEIGHT - ((_x % 2) * HEXAGON_HEIGHT_HALF));
            }
        }

        private List<Hexagon> _hexagons = new List<Hexagon>();
        private Texture2D _hexagonTexture;

        public HexagonBackground()
        {
            Game.WindowSizeChanged += HandleWindowSizeChanged;
            _hexagonTexture = Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.Common.Hexagon);
            Generate(true);
        }

        private void HandleWindowSizeChanged(object sender, EventArgs e)
        {
            Generate(false);
        }

        private void Generate(bool hasAnimation)
        {
            _hexagons.Clear();

            for (int x = -1; x < Game.ScreenBounds.Width / HEXAGON_WIDTH + 1; x++)
                for (int y = -1; y < Game.ScreenBounds.Height / HEXAGON_HEIGHT_HALF + 1; y++)
                    _hexagons.Add(new Hexagon(x, y, hasAnimation));
        }

        /// <summary>
        /// Draws the hexagon background. SpriteBatch has to be begun.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Hexagon hexagon in _hexagons.Where(x => x.Visible))
                spriteBatch.Draw(_hexagonTexture, hexagon.GetOffset(), new Color(255, 255, 255, (byte)hexagon.Alpha));
        }

        public void Update()
        {
            _hexagons.ForEach(x => x.Update());
        }
    }
}
