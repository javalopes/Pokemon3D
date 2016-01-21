using System;

namespace Pokemon3D.Common
{
    /// <summary>
    /// Provides a global <see cref="Random"/> instance for every "real" randomization to use.
    /// </summary>
    public class GlobalRandomProvider : Singleton<GlobalRandomProvider>
    {
        private Random _random;

        public GlobalRandomProvider()
        {
            _random = new Random();
        }

        public Random Rnd { get { return _random; } }
    }
}
