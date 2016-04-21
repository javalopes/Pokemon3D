using Pokemon3D.Entities;

namespace Pokemon3D.Screens
{
    /// <summary>
    /// Interface to be implemented by all screens that contain a world class intance.
    /// </summary>
    interface WorldContainer
    {
        World ActiveWorld { get; }
    }
}
