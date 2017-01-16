using Pokemon3D.Common;
using Pokemon3D.GameCore;

namespace Pokemon3D
{
    /// <summary>
    /// A static class to provide a shorthand access to the global game instance.
    /// Write "GameInstace" instead of "GameController.Instance".
    /// Import using "using static GameProvider;"
    /// </summary>
    static class GameProvider
    {
        // The name of this property cannot be changed to "Game" because it creates a conflict with the class name "Game"
        // in Microsoft.Xna.Framework, if this namespace is imported (using) to the source file.
        // Might consider renaming it to something like "GAME" (all caps) to differenciate it from the class name and make the name shorter.

        /// <summary>
        /// The active <see cref="GameController"/> instance.
        /// </summary>
        public static GameContext GameInstance => GameController.Instance;
    }
}

