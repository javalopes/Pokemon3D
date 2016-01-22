using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Common.Resources
{
    public interface Texture2DProvider
    {
        Texture2D GetTexture2D(string filePathContent);
    }
}
