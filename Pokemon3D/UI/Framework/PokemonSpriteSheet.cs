using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Pokemon3D.GameCore;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon3D.UI.Framework
{
    /// <summary>
    /// Manages a Pokémon sprite sheet.
    /// </summary>
    class PokemonSpriteSheet : GameObject
    {
        private const int FRAME_LENGTH = 2;

        private Texture2D[] _frames;
        private int _currentFrame = 0;
        private int _frameDelay = FRAME_LENGTH;

        public PokemonSpriteSheet(Texture2D spriteSheet, int frameSize)
        {
            ExtractFrames(spriteSheet, frameSize);
        }

        private void ExtractFrames(Texture2D spriteSheet, int frameSize)
        {
            List<Texture2D> frames = new List<Texture2D>();

            int pixelCountPerFrame = frameSize * frameSize;
            Color[] frameData;
            bool foundEmptyFrame = false;

            for (int y = 0; y < spriteSheet.Height; y += frameSize)
            {
                if (y + frameSize <= spriteSheet.Height && !foundEmptyFrame)
                {
                    for (int x = 0; x < spriteSheet.Width; x += frameSize)
                    {
                        if (x + frameSize <= spriteSheet.Width && !foundEmptyFrame)
                        {
                            frameData = new Color[pixelCountPerFrame];
                            spriteSheet.GetData(0, new Rectangle(x, y, frameSize, frameSize), frameData, 0, pixelCountPerFrame);

                            if (frameData.All(c => c.A == 0))
                            {
                                foundEmptyFrame = true;
                            }
                            else
                            {
                                Texture2D frame = new Texture2D(Game.GraphicsDevice, frameSize, frameSize);
                                frame.SetData(frameData);
                                frames.Add(frame);
                            }
                        }
                    }
                }
            }

            _frames = frames.ToArray();
        }

        public void Update()
        {
            _frameDelay--;
            if (_frameDelay <= 0)
            {
                _frameDelay = FRAME_LENGTH;
                _currentFrame++;
                if (_currentFrame >= _frames.Length)
                    _currentFrame = 0;
            }
        }

        public Texture2D CurrentFrame
        {
            get { return _frames[_currentFrame]; }
        }
    }
}
