using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;

namespace Pokemon3D.Rendering.Compositor.PostProcessing
{
    public abstract class IPostProcessEffectBase : IPostProcessEffect
    {
        private readonly EffectTechnique _technique;
        private readonly RenderTarget2D _target;
        private readonly IGameContext _context;
        private readonly Effect _postProcessEffect;

        protected EffectParameter SourceMapParameter { get; }
        protected EffectParameter InvScreenSizeParameter { get; }
        

        protected IPostProcessEffectBase(IGameContext context, Effect postProcessEffect, string techniqueName)
        {
            _context = context;
            _postProcessEffect = postProcessEffect;
            _technique = postProcessEffect.Techniques[techniqueName];
            if (_technique == null) throw new InvalidOperationException($"Provided effect processor has no technique called '{techniqueName}'");

            SourceMapParameter = postProcessEffect.Parameters["SourceMap"];
            InvScreenSizeParameter = postProcessEffect.Parameters["InvScreenSize"];
            
            _target = new RenderTarget2D(context.GetService<GraphicsDevice>(), context.ScreenBounds.Width, context.ScreenBounds.Height, false, SurfaceFormat.Color, DepthFormat.None);
        }

        public RenderTarget2D Process(SpriteBatch spriteBatch, Vector2 invScreenSize, RenderTarget2D source)
        {
            SourceMapParameter.SetValue(source);
            InvScreenSizeParameter.SetValue(invScreenSize);
            _postProcessEffect.CurrentTechnique = _technique;

            spriteBatch.GraphicsDevice.SetRenderTarget(_target);
            spriteBatch.Begin(effect: _postProcessEffect);
            spriteBatch.Draw(source, _context.ScreenBounds, Color.White);
            spriteBatch.End();

            return _target;
        }
    }
}