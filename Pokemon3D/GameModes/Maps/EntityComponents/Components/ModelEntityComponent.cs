using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.GameModes.Maps.EntityComponents.Components
{
    struct TextureRegion
    {
        public Texture2D Texture;
        public Rectangle? Rectangle;
    }

    class ModelEntityComponent : EntityComponent
    {
        private List<TextureRegion> _regions;
        private readonly DrawableElement _drawableElement;

        public ModelEntityComponent(EntityComponentDataCreationStruct parameters) : base(parameters)
        {
            _drawableElement = Parent.Game.Renderer.CreateDrawableElement(true);
            IsBillboard = GetDataOrDefault("IsBillboard", false);

            var modelReference = GetData<string>("MeshReference");
            var useTransparency = GetDataOrDefault("UseTransparency", false);

            if (!string.IsNullOrEmpty(modelReference))
            {
                var primitiveMesh = GameController.Instance.ActiveGameMode.GetPrimitiveMesh(modelReference);
                if (primitiveMesh != null)
                {
                    _drawableElement.Mesh = primitiveMesh;
                    _drawableElement.Material = new Material();
                }
                else
                {
                    var modelMesh = GameController.Instance.ActiveGameMode.GetModel(modelReference).First();
                    _drawableElement.Mesh = modelMesh.Mesh;
                    _drawableElement.Material = modelMesh.Material;
                    
                }

                _drawableElement.Material.Color = new Color(GetDataOrDefault("Shading", Vector3.One));
                _drawableElement.Material.CastShadow = true;
                _drawableElement.Material.ReceiveShadow = !useTransparency;
                _drawableElement.Material.UseTransparency = useTransparency;
                _drawableElement.Material.IsUnlit = false;
            }

            _regions = new List<TextureRegion>();
            var textures = GetEnumeratedData<string>("Texture");
            var regions = GetEnumeratedData<Rectangle>("Rectangle");

            var gameMode = Parent.Game.ActiveGameMode;
            if (textures.Length == 1)
            {
                AddTextureRegion(gameMode.GetTexture(textures[0]), regions.Length == 0 ? (Rectangle?) null : regions[0]);
            }
            else
            {
                for (var i = 0; i < textures.Length; i++)
                {
                    AddTextureRegion(gameMode.GetTexture(textures[0]), i >= regions.Length ? (Rectangle?)null : regions[i]);
                }
            }

            SetTexture(0);

            _drawableElement.EndInitialzing();
        }

        public ModelEntityComponent(Entity parent, Mesh mesh, Material material, bool isBillboard) : base(parent)
        {
            _drawableElement = parent.Game.Renderer.CreateDrawableElement(true);
            _drawableElement.Material = material;
            _drawableElement.Mesh = mesh;
            _drawableElement.EndInitialzing();
            IsBillboard = isBillboard;
        }

        public override void OnIsActiveChanged()
        {
            _drawableElement.IsActive = IsActive;
        }

        public void AddTextureRegion(Texture2D texture, Rectangle? region)
        {
            _regions.Add(new TextureRegion
            {
                Texture = texture,
                Rectangle = region
            });
        }

        public bool IsBillboard
        {
            get { return _drawableElement.IsBillboard; }
            set { _drawableElement.IsBillboard = value; }
        }

        public void SetTexture(int index)
        {
            if (index >= _regions.Count) return;

            var region = _regions[index];
            _drawableElement.Material.DiffuseTexture = region.Texture;
            if (region.Rectangle.HasValue)
            {
                var rectangle = region.Rectangle.Value;
                _drawableElement.Material.TexcoordOffset = region.Texture.GetTexcoordsFromPixelCoords(rectangle.X, rectangle.Y);
                _drawableElement.Material.TexcoordScale = region.Texture.GetTexcoordsFromPixelCoords(rectangle.Width, rectangle.Height);
            }
            else
            {
                _drawableElement.Material.TexcoordOffset = Vector2.Zero;
                _drawableElement.Material.TexcoordScale = Vector2.One;
            }
            
        }

        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);
            _drawableElement.WorldMatrix = Parent.WorldMatrix;
            _drawableElement.Scale = Parent.Scale;

            if (_drawableElement.Mesh != null)
            {
                var box = _drawableElement.Mesh.LocalBounds;
                box.Min = box.Min * Parent.Scale;
                box.Max = box.Max * Parent.Scale;

                if (IsBillboard)
                {
                    box.Min.X = MathHelper.Min(box.Min.X, box.Min.Z);
                    box.Min.Z = box.Min.X;
                    box.Max.X = MathHelper.Max(box.Max.X, box.Max.Z);
                    box.Max.Z = box.Max.X;
                }

                box.Min += Parent.GlobalPosition;
                box.Max += Parent.GlobalPosition;

                _drawableElement.BoundingBox = box;
                _drawableElement.GlobalPosition = Parent.GlobalPosition;
            }
        }

        public Material Material => _drawableElement.Material;
    }
}
