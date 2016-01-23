using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.Data
{
    public interface GameModeDataProvider
    {
        Mesh GetPrimitiveMesh(string primitiveName);

        string TexturePath { get; }
    }
}