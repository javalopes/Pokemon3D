using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.Rendering
{
    public class Skybox : GameContextObject
    {
        private readonly Mesh _skyBoxModel;
        
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

        public Skybox(GameContext gameContext) : base(gameContext)
        {
            DrawableElement = new DrawableElement(0, false, null);

            var height = 1.0f / 3.0f;
            var width = 0.25f;
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
            
            _skyBoxModel = new Mesh(GameContext.GetService<GraphicsDevice>(), Primitives.GenerateCubeData(coords));
            DrawableElement.Mesh = _skyBoxModel;
            DrawableElement.Material = new Material
            {
                CastShadow = false,
                ReceiveShadow = false,
                UseTransparency = false
            };
            Scale = 1.0f;
        }

        internal void Update(Camera camera)
        {
            DrawableElement.WorldMatrix = Matrix.CreateScale(-Scale) * Matrix.CreateTranslation(camera.GlobalPosition);
        }
    }
}