using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace Pokemon3D.Rendering.GUI.ItemDescriptors
{
    class TextBlockSkinItemDescriptor : SkinItemDescriptor
    {
        public string NodeName { get { return "TextBlock"; } }
        
        public Texture2D SkinTexture { get; set; }
        public SpriteFont BigFont { get; set; }
        public SpriteFont NormalFont { get; set; }

        public void Deserialize(XmlElement element)
        {

        }
    }
}
