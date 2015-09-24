﻿using Microsoft.Xna.Framework;

namespace Pokémon3D.UI.Screens
{
    /// <summary>
    /// A screen to represent a scene in the game.
    /// </summary>
    interface Screen
    {
        /// <summary>
        /// Raises the Draw event.
        /// </summary>
        void OnDraw(GameTime gameTime);

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
        void OnOpening();
    }
}