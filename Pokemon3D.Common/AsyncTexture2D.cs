using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Common
{
    public class AsyncTexture2D
    {
        public Texture2D Texture { get; private set; }
        public bool LoadingComplete { get; private set; }

        public AsyncTexture2D()
        {
            Texture = null;
            LoadingComplete = false;
        }


        public AsyncTexture2D(Texture2D alreadyCached)
        {
            SetTexture(alreadyCached);
        }

        public void SetTexture(Texture2D texture)
        {
            Texture = texture;
            LoadingComplete = true;
        }
    }
}
