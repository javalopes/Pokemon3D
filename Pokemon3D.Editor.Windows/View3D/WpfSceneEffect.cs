using Pokemon3D.Rendering.Compositor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace Pokemon3D.Editor.Windows.View3D
{
    class WpfSceneEffect : SceneEffect
    {
        private readonly Effect _basicEffect;

        private Dictionary<int, EffectTechnique> _effectsByLightingFlags;

        private readonly EffectTechnique _shadowCasterTechnique;
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
        private readonly EffectParameter _materialColor;

        public WpfSceneEffect(GraphicsDevice device, string folderPath)
        {
            _basicEffect = new Effect(device, File.ReadAllBytes(Path.Combine(folderPath, "BasicEffect.mgfx")));
            PostProcessingEffect = null;
            ShadowMapDebugEffect = null;

            _shadowCasterTechnique = _basicEffect.Techniques["ShadowCaster"];
            _shadowCasterTransparentTechnique = _basicEffect.Techniques["ShadowCasterTransparent"];
            

            _effectsByLightingFlags = new Dictionary<int, EffectTechnique>()
            {
                { LightTechniqueFlag.Lit | LightTechniqueFlag.ReceiveShadows,_basicEffect.Techniques["LitNoTextureShadowReceiver"] },
                { LightTechniqueFlag.Lit | LightTechniqueFlag.ReceiveShadows | LightTechniqueFlag.SoftShadows , _basicEffect.Techniques["LitNoTextureShadowReceiverPCF"] },
                { LightTechniqueFlag.Lit | LightTechniqueFlag.UseTexture, _basicEffect.Techniques["Lit"] },
                { LightTechniqueFlag.Lit | LightTechniqueFlag.UseTexture | LightTechniqueFlag.ReceiveShadows, _basicEffect.Techniques["LitShadowReceiver"] },
                { LightTechniqueFlag.Lit | LightTechniqueFlag.UseTexture | LightTechniqueFlag.ReceiveShadows | LightTechniqueFlag.SoftShadows, _basicEffect.Techniques["LitShadowReceiverPCF"] },
                { 0, _basicEffect.Techniques["UnlitNoTexture"] },
                { LightTechniqueFlag.UseTexture, _basicEffect.Techniques["Unlit"] },
                { LightTechniqueFlag.UseTexture | LightTechniqueFlag.LinearTextureSampling, _basicEffect.Techniques["UnlitLinearSampled"] },
            };

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
            _materialColor = _basicEffect.Parameters["MaterialColor"];
        }

        public Effect ShadowMapDebugEffect { get; }

        public void ActivateShadowDepthMapPass(bool transparent)
        {
            _basicEffect.CurrentTechnique = transparent ? _shadowCasterTransparentTechnique : _shadowCasterTechnique;
        }

        public void ActivateLightingTechnique(int flags)
        {
            _basicEffect.CurrentTechnique = _effectsByLightingFlags[flags];
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
            set { _shadowScale.SetValue(value); }
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
            set { _ambientLight.SetValue(value); }
        }

        public Color MaterialColor
        {
            get { return new Color(_materialColor.GetValueVector4()); }
            set
            {
                _materialColor.SetValue(value.ToVector4());
            }
        }
    }
}
