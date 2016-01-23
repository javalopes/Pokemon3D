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
        private class Hexagon
        {
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
                _targetAlpha = Common.GlobalRandomProvider.Instance.Rnd.Next(100, 200);

                if (hasAnimation)
                {
                    _delay = y * 0.2f;
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
                        if (Math.Abs(Alpha - _targetAlpha) < 10f)
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
                            if (Common.GlobalRandomProvider.Instance.Rnd.Next(0, 40000) == 0)
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
                return new Vector2(_x * 26, _y * 31 - ((_x % 2) * 15));
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

            for (int x = -1; x < Game.ScreenBounds.Width / 26 + 1; x++)
                for (int y = -1; y < Game.ScreenBounds.Height / 15 + 1; y++)
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
