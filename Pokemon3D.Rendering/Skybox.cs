using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.Rendering
{
    public class Skybox
    {
        public DrawableElement DrawableElement { get; }

        public Texture2D Texture
        {
            get { return DrawableElement.Material.DiffuseTexture; }
            set { DrawableElement.Material.DiffuseTexture = value; }
        }

        public float Scale
        {
            get; set;
        }

        public Skybox(GraphicsDevice graphicsDevice)
        {
            DrawableElement = new DrawableElement(0);

            const float height = 1.0f / 3.0f;
            const float width = 0.25f;
            const float threshold = 0.001f;
            var coords = new[]
            {
                new Vector2(1.0f*width+threshold,1.0f*height+threshold), new Vector2(width-threshold*2, height-threshold*2),
                new Vector2(3.0f*width+threshold,1.0f*height+threshold), new Vector2(width-threshold*2, height-threshold*2),
                new Vector2(2.0f*width+threshold,1.0f*height+threshold), new Vector2(width-threshold*2, height-threshold*2),
                new Vector2(0.0f*width+threshold,1.0f*height+threshold), new Vector2(width-threshold*2, height-threshold*2),
                new Vector2(1.0f*width+threshold,0.0f*height+threshold), new Vector2(width-threshold*2, height-threshold*2),
                new Vector2(1.0f*width+threshold,2.0f*height+threshold), new Vector2(width-threshold*2, height-threshold*2),
            };

            DrawableElement.Mesh = new Mesh(graphicsDevice, Primitives.GenerateCubeData(coords));
            DrawableElement.Material = new Material
            {
                CastShadow = false,
                ReceiveShadow = false,
                UseTransparency = false,
                UseLinearTextureSampling = true,
                IsUnlit = true
            };
            Scale = 1.0f;
        }

        internal void Update(Camera camera)
        {
            DrawableElement.WorldMatrix = Matrix.CreateScale(-Scale) * Matrix.CreateTranslation(camera.GlobalPosition);
        }
    }
}