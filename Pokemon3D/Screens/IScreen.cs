using Microsoft.Xna.Framework;
using Pokemon3D.Rendering.UI;

namespace Pokemon3D.Screens
{
    /// <summary>
    /// A screen to represent a scene in the game.
    /// </summary>
    internal interface IScreen
    {
        /// <summary>
        /// This is called before the internal renderer draws all entities.
        /// </summary>
        void OnEarlyDraw(GameTime gameTime);

        /// <summary>
        /// This is called after the internal renderer draws all entities. This is useful for drawing custom overlays.
        /// </summary>
        /// <param name="gameTime"></param>
        void OnLateDraw(GameTime gameTime);

        /// <summary>
        /// Raises the Update event.
        /// </summary>
        void OnUpdate(GameTime gameTime);

        /// <summary>
        /// Raises the Closing event.
        /// </summary>
        void OnClosing();

        /// <summary>
        /// Raising the Opening event.
        /// </summary>
        /// <param name="enterInformation">Context information from previous screen</param>
        void OnOpening(object enterInformation);

        /// <summary>
        /// Adds an ui overlay container for possible drawing
        /// </summary>
        /// <param name="overlay">overlay</param>
        /// <returns>overlay</returns>
        UiOverlay AddOverlay(UiOverlay overlay);

        /// <summary>
        /// Removes overlay
        /// </summary>
        /// <param name="overlay">overlay</param>
        void RemoveOverlay(UiOverlay overlay);
    }
}