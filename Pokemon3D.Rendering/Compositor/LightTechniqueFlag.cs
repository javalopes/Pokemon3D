namespace Pokemon3D.Rendering.Compositor
{
    /// <summary>
    /// Defines Lighting Flags
    /// </summary>
    public static class LightTechniqueFlag
    {
        /// <summary>
        /// Object will be lit dynamically.
        /// </summary>
        public static int Lit = 1;
        /// <summary>
        /// Object can receive a shadow.
        /// </summary>
        public static int ReceiveShadows = 2;
        /// <summary>
        /// When object cna receive a shadows this should be a soft one.
        /// </summary>
        public static int SoftShadows = 4;
        /// <summary>
        /// Object uses a texture.
        /// </summary>
        public static int UseTexture = 8;
        /// <summary>
        /// Object uses linear sampling.
        /// </summary>
        public static int LinearTextureSampling = 16;
    }
}
