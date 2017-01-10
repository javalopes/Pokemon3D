using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Content;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Compositor;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.Screens
{
    internal class WindowsEffectProcessor : EffectProcessor
    {
        private readonly Effect _basicEffect;

        private readonly Dictionary<int, EffectTechnique> _effectsByLightingFlags;

        private readonly EffectTechnique _shadowCasterTechnique;
        private readonly EffectTechnique _shadowCasterTransparentTechnique;

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

        public Matrix WorldLight
        {
            get { return _worldLight.GetValueMatrix(); }
            set { _worldLight.SetValue(value); }
        }

        public Vector4 AmbientLight
        {
            get { return _ambientLight.GetValueVector4(); }
            set { _ambientLight.SetValue(value); }
        }

        public Effect PostProcessingEffect { get; }

        public WindowsEffectProcessor(ContentManager content)
        {
            _basicEffect = content.Load<Effect>(ResourceNames.Effects.BasicEffect);
            PostProcessingEffect = content.Load<Effect>(ResourceNames.Effects.PostProcessing);

            _shadowCasterTechnique = _basicEffect.Techniques["ShadowCaster"];
            _shadowCasterTransparentTechnique = _basicEffect.Techniques["ShadowCasterTransparent"];

            _effectsByLightingFlags = new Dictionary<int, EffectTechnique>
            {
                { LightTechniqueFlag.Lit | LightTechniqueFlag.ReceiveShadows,_basicEffect.Techniques["LitNoTextureShadowReceiver"] },
                { LightTechniqueFlag.Lit | LightTechniqueFlag.ReceiveShadows | LightTechniqueFlag.SoftShadows , _basicEffect.Techniques["LitNoTextureShadowReceiverPCF"] },
                { LightTechniqueFlag.Lit | LightTechniqueFlag.UseTexture, _basicEffect.Techniques["Lit"] },
                { LightTechniqueFlag.Lit | LightTechniqueFlag.UseTexture | LightTechniqueFlag.ReceiveShadows, _basicEffect.Techniques["LitShadowReceiver"] },
                { LightTechniqueFlag.Lit | LightTechniqueFlag.UseTexture | LightTechniqueFlag.ReceiveShadows | LightTechniqueFlag.SoftShadows, _basicEffect.Techniques["LitShadowReceiverPCF"] },
                { 0, _basicEffect.Techniques["UnlitNoTexture"] },
                { LightTechniqueFlag.UseTexture, _basicEffect.Techniques["Unlit"] },
                { LightTechniqueFlag.UseTexture| LightTechniqueFlag.ReceiveShadows, _basicEffect.Techniques["UnlitShadowReceiver"] },
                { LightTechniqueFlag.UseTexture| LightTechniqueFlag.ReceiveShadows| LightTechniqueFlag.SoftShadows, _basicEffect.Techniques["UnlitShadowReceiverPCF"] },
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

        public void SetLights(IList<Light> lights)
        {
            var light = lights.First();
            _shadowMap.SetValue(light.ShadowMap);
            _lightViewProjection.SetValue(light.LightViewMatrix);
            _shadowScale.SetValue(1.0f / light.ShadowMap.Width);
            _lightDirection.SetValue(light.Direction);
            _ambientIntensity.SetValue(light.AmbientIntensity);
            _diffuseIntensity.SetValue(light.DiffuseIntensity);
        }

        public EffectPassCollection ApplyByMaterial(Material material, RenderSettings renderSettings)
        {            
            _diffuseTexture.SetValue(material.DiffuseTexture);
            _texcoordScale.SetValue(material.TexcoordScale);
            _texcoordOffset.SetValue(material.TexcoordOffset);
            _materialColor.SetValue(material.Color.ToVector4());

            var lightingTypeFlags = material.GetLightingTypeFlags(renderSettings);

            return _effectsByLightingFlags[lightingTypeFlags].Passes;
        }

        public EffectPassCollection GetShadowDepthPass(Material material)
        {
            return material.UseTransparency
                ? _shadowCasterTransparentTechnique.Passes
                : _shadowCasterTechnique.Passes;
        }
    }
}