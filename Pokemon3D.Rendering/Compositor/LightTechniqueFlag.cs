using System;

namespace Pokemon3D.Rendering.Compositor
{
    /// <summary>
    /// Defines Lighting Flags
    /// </summary>
    [Flags]
    public enum LightTechniqueFlag
    {
        /// <summary>
        /// Object will be lit dynamically.
        /// </summary>
        Lit = 1,
        /// <summary>
        /// Object can receive a shadow.
        /// </summary>
        ReceiveShadows = 2,
        /// <summary>
        /// When object cna receive a shadows this should be a soft one.
        /// </summary>
        SoftShadows = 4,
        /// <summary>
        /// Object uses a texture.
        /// </summary>
        UseTexture = 8,
        /// <summary>
        /// Object uses linear sampling.
        /// </summary>
        LinearTextureSampling = 16
    }
}
