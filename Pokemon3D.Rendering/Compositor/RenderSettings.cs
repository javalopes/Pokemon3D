namespace Pokemon3D.Rendering.Compositor
{
    /// <summary>
    /// Configuration of Rendering Settings.
    /// </summary>
    public class RenderSettings
    {
        /// <summary>
        /// If shadows are enabled.
        /// </summary>
        public bool EnableShadows { get; set; }

        /// <summary>
        /// Soft Shadows looks better but are slower.
        /// </summary>
        public bool EnableSoftShadows { get; set; }

        /// <summary>
        /// Size of Shadow Map in Pixels for each dimension.
        /// </summary>
        public int ShadowMapSize { get; set; }
    }
}