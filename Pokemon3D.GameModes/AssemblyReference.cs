using System.Reflection;

namespace Pokemon3D.GameModes
{
    public static class AssemblyReference
    {
        /// <summary>
        /// Returns the assembly type of this assembly.
        /// </summary>
        public static Assembly Get() => typeof(AssemblyReference).Assembly;
    }
}
