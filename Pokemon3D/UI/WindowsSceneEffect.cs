using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.Compositor;

namespace Pokemon3D.UI
{
    class WindowsSceneEffect : SceneEffect
    {
        private readonly Effect _basicEffect;
        private readonly EffectTechnique _shadowCasterTechnique;
        private readonly EffectTechnique _litShadowReceiver;
        private readonly EffectTechnique _litShadowReceiverPcf; 
        private readonly EffectTechnique _litTechnique;
        private readonly EffectTechnique _unlitTechnique;
        private readonly EffectTechnique _unlitLinearSampledTechnique;
        private EffectTechnique _shadowCasterTransparentTechnique;

        private readonly EffectParameter _lightViewProjection;
        private readonly EffectParameter _world;
        private readonly EffectParameter _worldLight;
        private readonly EffectParameter _view;
        private readonly EffectParameter _projection;
        private readonly EffectParameter _lightDirection;
        private readonly EffectParameter _shadowMap;
        private readonly EffectParameter _diffuseTexture;
        private readonly EffectParameter _texcoordOffset;
        private readonly EffectParameter _texcoordScale;
        private readonly EffectParameter _ambientLight;
        private readonly EffectParameter _ambientIntensity;
        private readonly EffectParameter _diffuseIntensity;
        private readonly EffectParameter _shadowScale;
        
        public WindowsSceneEffect(ContentManager content)
        {
            _basicEffect = content.Load<Effect>(ResourceNames.Effects.BasicEffect);
            PostProcessingEffect = content.Load<Effect>(ResourceNames.Effects.PostProcessing);

            _litTechnique = _basicEffect.Techniques["Lit"];
            _shadowCasterTechnique = _basicEffect.Techniques["ShadowCaster"];
            _shadowCasterTransparentTechnique = _basicEffect.Techniques["ShadowCasterTransparent"];
            _litShadowReceiver = _basicEffect.Techniques["LitShadowReceiver"];
            _litShadowReceiverPcf = _basicEffect.Techniques["LitShadowReceiverPCF"];
            _unlitTechnique = _basicEffect.Techniques["Unlit"];
            _unlitLinearSampledTechnique = _basicEffect.Techniques["UnlitLinearSampled"];
            ShadowMapDebugEffect = content.Load<Effect>(ResourceNames.Effects.DebugShadowMap);

            _lightViewProjection = _basicEffect.Parameters["LightViewProjection"];
            _world = _basicEffect.Parameters["World"];
            _worldLight = _basicEffect.Parameters["WorldLight"];
            _view = _basicEffect.Parameters["View"];
            _projection = _basicEffect.Parameters["Projection"];
            _lightDirection = _basicEffect.Parameters["LightDirection"];
            _shadowMap = _basicEffect.Parameters["ShadowMap"];
            _diffuseTexture = _basicEffect.Parameters["DiffuseTexture"];
            _texcoordOffset = _basicEffect.Parameters["TexcoordOffset"];
            _texcoordScale = _basicEffect.Parameters["TexcoordScale"];
            _ambientLight = _basicEffect.Parameters["AmbientLight"];
            _ambientIntensity = _basicEffect.Parameters["AmbientIntensity"];
            _diffuseIntensity = _basicEffect.Parameters["DiffuseIntensity"];
            _shadowScale = _basicEffect.Parameters["ShadowScale"];
        }

        public Effect ShadowMapDebugEffect { get; }
        
        public void ActivateShadowDepthMapPass(bool transparent)
        {
            _basicEffect.CurrentTechnique = transparent ? _shadowCasterTransparentTechnique : _shadowCasterTechnique;
        }

        public void ActivateLightingTechnique(bool linearSampling, bool unlit, bool receiveShadows, bool pcfShadows)
        {
            if (unlit)
            {
                _basicEffect.CurrentTechnique = linearSampling ? _unlitLinearSampledTechnique : _unlitTechnique;
            }
            else
            {
                _basicEffect.CurrentTechnique = receiveShadows ? (pcfShadows ? _litShadowReceiverPcf : _litShadowReceiver) : _litTechnique;
            }
        }

        public Matrix LightViewProjection
        {
            get { return _lightViewProjection.GetValueMatrix(); }
            set { _lightViewProjection.SetValue(value); }
        }

        public Matrix WorldLight
        {
            get { return _worldLight.GetValueMatrix(); }
            set { _worldLight.SetValue(value); }
        }

        public float DiffuseIntensity
        {
            get { return _diffuseIntensity.GetValueSingle(); }
            set { _diffuseIntensity.SetValue(value); }
        }

        public Matrix World
        {
            get { return _world.GetValueMatrix(); }
            set { _world.SetValue(value); }
        }

        public Matrix View
        {
            get { return _view.GetValueMatrix(); }
            set { _view.SetValue(value); }
        }

        public Matrix Projection
        {
            get { return _projection.GetValueMatrix(); }
            set { _projection.SetValue(value); }
        }

        public Vector3 LightDirection
        {
            get { return _lightDirection.GetValueVector3(); }
            set { _lightDirection.SetValue(value); }
        }

        public float AmbientIntensity
        {
            get { return _ambientIntensity.GetValueSingle(); }
            set { _ambientIntensity.SetValue(value); }
        }

        public Texture2D ShadowMap
        {
            get { return _shadowMap.GetValueTexture2D(); }
            set { _shadowMap.SetValue(value); }
        }

        public float ShadowScale
        {
            get { return _shadowScale.GetValueSingle(); }
            set { _shadowScale.SetValue(value);}
        }

        public Texture2D DiffuseTexture
        {
            get { return _diffuseTexture.GetValueTexture2D(); }
            set { _diffuseTexture.SetValue(value); }
        }

        public Effect PostProcessingEffect { get; }

        public Vector2 TexcoordOffset
        {
            get { return _texcoordOffset.GetValueVector2(); }
            set { _texcoordOffset.SetValue(value); }
        }

        public Vector2 TexcoordScale
        {
            get { return _texcoordScale.GetValueVector2(); }
            set { _texcoordScale.SetValue(value); }
        }

        public EffectPassCollection CurrentTechniquePasses => _basicEffect.CurrentTechnique.Passes;

        public Vector4 AmbientLight
        {
            get { return _ambientLight.GetValueVector4(); }
            set { _ambientLight.SetValue(value);}
        }
    }
}
