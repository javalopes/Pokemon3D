using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.Compositor.PostProcessing
{
    public class PostProcess
    {
        private readonly List<IPostProcessEffect> _effects = new List<IPostProcessEffect>();

        internal PostProcess()
        {
            
        }

        public void Add(IPostProcessEffect effect)
        {
            _effects.Add(effect);
        }

        internal RenderTarget2D ProcessChain(SpriteBatch spriteBatch, Vector2 invScreenSize, RenderTarget2D startingSource)
        {
            var currentSource = startingSource;
            foreach (var postProcessEffect in _effects)
            {
                currentSource = postProcessEffect.Process(spriteBatch, invScreenSize, currentSource);
            }
            return currentSource;
        }

        public bool IsActive { get; set; }

        public bool ShouldProcess => IsActive && _effects.Count > 0;
    }
}
