using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Common.Extensions;
using Pokemon3D.GameModes;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Entities.System.Components
{
    [JsonComponentId("visualmodel")]
    internal class ModelEntityComponent : EntityComponent
    {
        private List<TextureRegion> _regions;
        private DrawableElement _drawableElement;

        public List<TextureRegion> Regions => _regions;
        public Material Material => _drawableElement.Material;
        public Mesh Mesh => _drawableElement.Mesh;

        private bool IsBillboard
        {
            get { return _drawableElement.IsBillboard; }
            set { _drawableElement.IsBillboard = value; }
        }

        private ModelEntityComponent(Entity parent) : base(parent)
        {
            
        }

        public ModelEntityComponent(EntityComponentDataCreationStruct parameters) : base(parameters)
        {
            _drawableElement = GameInstance.GetService<SceneRenderer>().CreateDrawableElement(true);
            IsBillboard = GetDataOrDefault("IsBillboard", false);

            var modelReference = GetData<string>("MeshReference");
            var useTransparency = GetDataOrDefault("UseTransparency", false);
            var gameMode = GameInstance.GetService<GameModeManager>().ActiveGameMode;

            if (!string.IsNullOrEmpty(modelReference))
            {
                var primitiveMesh = gameMode.GetPrimitiveMesh(modelReference);
                if (primitiveMesh != null)
                {
                    _drawableElement.Mesh = primitiveMesh;
                    _drawableElement.Material = new Material();
                }
                else
                {
                    var modelMesh = gameMode.GetModel(modelReference).First();
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

            if (textures.Length == 1)
            {
                AddTextureRegion(gameMode.GetTexture(textures[0]), regions.Length == 0 ? (Rectangle?)null : regions[0]);
            }
            else
            {
                for (var i = 0; i < textures.Length; i++)
                {
                    AddTextureRegion(gameMode.GetTexture(textures[0]), i >= regions.Length ? (Rectangle?)null : regions[i]);
                }
            }

            SetTexture(0);

            _drawableElement.IsActive = Parent.IsActive;
            if (!Parent.IsInitializing) _drawableElement.EndInitialzing();
        }

        public ModelEntityComponent(Entity parent, Mesh mesh, Material material, bool isBillboard) : base(parent)
        {
            _drawableElement = GameInstance.GetService<SceneRenderer>().CreateDrawableElement(true);
            _drawableElement.Material = material;
            _drawableElement.Mesh = mesh;
            IsBillboard = isBillboard;
        }

        public override void OnIsActiveChanged()
        {
            _drawableElement.IsActive = IsActive;
        }

        public override void OnInitialized()
        {
            _drawableElement.EndInitialzing();
        }

        public override EntityComponent Clone(Entity target)
        {
            var clonedComponent = new ModelEntityComponent(target);
            clonedComponent._regions = _regions != null ? new List<TextureRegion>(_regions) : null;
            clonedComponent._drawableElement = GameInstance.GetService<SceneRenderer>().CreateDrawableElement(target.IsInitializing);
            clonedComponent._drawableElement.Material = Material;
            clonedComponent._drawableElement.Mesh = Mesh;
            clonedComponent.IsBillboard = IsBillboard;
            clonedComponent.IsActive = target.IsActive;

            return clonedComponent;
        }

        public override void OnComponentRemove()
        {
            GameInstance.GetService<SceneRenderer>().RemoveDrawableElement(_drawableElement);
            _drawableElement = null;
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _drawableElement.WorldMatrix = Parent.WorldMatrix;
            _drawableElement.Scale = Parent.Scale;
            _drawableElement.GlobalPosition = Parent.GlobalPosition;

            if (_drawableElement.Mesh != null)
            {
                if (IsBillboard)
                {
                    var box = _drawableElement.Mesh.LocalBounds;
                    box.Min = box.Min * Parent.Scale;
                    box.Max = box.Max * Parent.Scale;
                    box.Min.X = MathHelper.Min(box.Min.X, box.Min.Z);
                    box.Min.Z = box.Min.X;
                    box.Max.X = MathHelper.Max(box.Max.X, box.Max.Z);
                    box.Max.Z = box.Max.X;
                    box.Min += Parent.GlobalPosition;
                    box.Max += Parent.GlobalPosition;
                    _drawableElement.BoundingBox = box;
                }
                else
                {
                    _drawableElement.UpdateBounds();
                }
            }
        }

        private void AddTextureRegion(Texture2D texture, Rectangle? region)
        {
            _regions.Add(new TextureRegion
            {
                Texture = texture,
                Rectangle = region
            });
        }
    }
}
