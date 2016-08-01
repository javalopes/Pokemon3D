using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Screens.Transitions
{
    internal interface ScreenTransition
    {
        void StartTransition(Texture2D source, Texture2D target);

        bool IsFinished { get; }

        void Update(GameTime gameTime);

        void Draw();
    }
}
