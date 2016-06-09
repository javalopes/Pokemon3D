namespace Pokemon3D.Rendering
{
    /// <summary>
    /// Light Types. currently just directional lights supported.
    /// </summary>
    public enum LightType
    {
        /// <summary>
        /// Light with a direction.
        /// </summary>
        Directional,

        /// <summary>
        /// Light with 3D World position and Range.
        /// </summary>
        Point,
    }
}