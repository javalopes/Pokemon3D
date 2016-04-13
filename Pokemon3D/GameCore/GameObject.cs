namespace Pokemon3D.GameCore
{
    // note:
    // For now, no classes inherit from this type.
    // it was just used to gain easier access to the singleton instance of GameController.
    // this is now handled by GameProvider, which is imported into the files with a static using.
    // only use this type as a common super class, when appropriate.

    abstract class GameObject
    {
        public GameController Game => GameController.Instance;
    }
}
