using System;

namespace Pokemon3D.Common
{
    /// <summary>
    /// Provides a global <see cref="Random"/> instance for every "real" randomization to use.
    /// </summary>
    public class GlobalRandomProvider : Singleton<GlobalRandomProvider>
    {
        public GlobalRandomProvider()
        {
            Rnd = new Random();
        }

        public Random Rnd { get; }
    }
}
