using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.Rendering.Compositor
{
    public interface EffectProcessor
    {
        /// <summary>
        /// World Matrix to light. Differs to World for Billboards.
        /// </summary>
        Matrix WorldLight { get; set; }

        /// <summary>
        /// Sets Ambient Component.
        /// </summary>
        Vector4 AmbientLight { get; set; }
        
        /// <summary>
        /// World Matrix for normal mesh rendering with lighting.
        /// </summary>
        Matrix World { get; set; }

        /// <summary>
        /// Camera Matrix for normal mesh rendering with lighting.
        /// </summary>
        Matrix View { get; set; }

        /// <summary>
        /// Projection Matrix for normal mesh rendering with lighting.
        /// </summary>
        Matrix Projection { get; set; }
        
        /// <summary>
        /// Sets all lights for rendering.
        /// </summary>
        /// <param name="light">lights to set</param>
        void SetLights(IList<Light> light);

        /// <summary>
        /// Passes a material and returns passes to run for effect.
        /// </summary>
        /// <param name="material">Material defining shader permutation to run.</param>
        /// <param name="renderSettings"></param>
        /// <returns>Effect Passes to iterate</returns>
        EffectPassCollection ApplyByMaterial(Material material, RenderSettings renderSettings);

        EffectPassCollection GetShadowDepthPass(Material material);

        Effect PostProcessingEffect { get; }
    }
}