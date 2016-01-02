using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace Pokemon3D.Rendering.GUI
{
    internal interface SkinItemDescriptor
    {
        string NodeName { get;}
        Texture2D SkinTexture { get; set; }
        SpriteFont BigFont { get; set; }
        SpriteFont NormalFont { get; set; }

        void Deserialize(XmlElement element);
    }
}
