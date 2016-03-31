using Microsoft.Xna.Framework;
using Pokemon3D.Common.Extensions;
using Pokemon3D.DataModel.GameMode.Map.Entities;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.GameModes.Maps.EntityComponents.Components
{
    class ModelEntityComponent : EntityComponent
    {
        private EntityRenderModeModel _dataModel;
        private DrawableElement _drawbaleElement;

        public ModelEntityComponent(Entity parent, EntityRenderModeModel dataModel) : base(parent)
        {
            //todo: remove this method, because this is a task for the creator of this entity.
            _dataModel = dataModel;
            InitializeRenderingData(dataModel.IsBillboard, dataModel.RenderMethod, dataModel.PrimitiveModelId, dataModel.Shading.GetVector3(), dataModel.UseTransparency, dataModel.ModelPath);
        }

        public ModelEntityComponent(Entity parent, Mesh mesh, Material material, bool isBillboard) : base(parent)
        {
            _drawbaleElement = parent.Game.Renderer.CreateDrawableElement(true);
            _drawbaleElement.Material = material;
            _drawbaleElement.Mesh = mesh;
            _drawbaleElement.EndInitialzing();
            IsBillboard = isBillboard;
        }

        private void InitializeRenderingData(bool isBillboard, RenderMethod renderMethod, string primitiveModelId, Vector3 shadingVector, bool useTransparency, string modelPath)
        {
            _drawbaleElement = Parent.Game.Renderer.CreateDrawableElement(true);
            _drawbaleElement.Material = new Material();
            IsBillboard = isBillboard;

            if (renderMethod == RenderMethod.Primitive)
            {
                //todo: that might not be a good idea.
                _drawbaleElement.Mesh = GameController.Instance.ActiveGameMode.GetPrimitiveMesh(primitiveModelId);
                _drawbaleElement.Material.Color = new Color(shadingVector);
                _drawbaleElement.Material.CastShadow = true;
                _drawbaleElement.Material.ReceiveShadow = !useTransparency;
                _drawbaleElement.Material.UseTransparency = useTransparency;
                _drawbaleElement.Material.IsUnlit = false;

                SetTexture(0);
            }
            else
            {
                //todo: that might not be a good idea.
                var models = GameController.Instance.ActiveGameMode.GetModel(modelPath);
                if (models.Length >= 1)
                {
                    AttachModelToDrawableElement(_drawbaleElement, models[0], shadingVector, useTransparency);
                }
            }

            _drawbaleElement.EndInitialzing();
        }

        public bool IsBillboard
        {
            get { return _drawbaleElement.IsBillboard; }
            set { _drawbaleElement.IsBillboard = value; }
        }

        public void SetTexture(int index)
        {
            var texture = _dataModel.Textures[index];

            //todo: that might not be a good idea.
            var diffuseTexture = GameController.Instance.ActiveGameMode.GetTexture(texture.Source);
            _drawbaleElement.Material.DiffuseTexture = diffuseTexture;

            if (texture.Rectangle != null)
            {
                _drawbaleElement.Material.TexcoordOffset = diffuseTexture.GetTexcoordsFromPixelCoords(texture.Rectangle.X, texture.Rectangle.Y);
                _drawbaleElement.Material.TexcoordScale = diffuseTexture.GetTexcoordsFromPixelCoords(texture.Rectangle.Width, texture.Rectangle.Height);
            }
            else
            {
                _drawbaleElement.Material.TexcoordOffset = Vector2.Zero;
                _drawbaleElement.Material.TexcoordScale = Vector2.One;
            }
        }

        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);
            _drawbaleElement.WorldMatrix = Parent.WorldMatrix;
            _drawbaleElement.Scale = Parent.Scale;

            if (_drawbaleElement.Mesh != null)
            {
                var box = _drawbaleElement.Mesh.LocalBounds;
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

                _drawbaleElement.BoundingBox = box;
                _drawbaleElement.GlobalPosition = Parent.GlobalPosition;
            }
        }

        public Material Material { get { return _drawbaleElement.Material; } }

        private void AttachModelToDrawableElement(DrawableElement drawbaleElement, ModelMesh modelMesh, Vector3 shading, bool useTransparency)
        {
            drawbaleElement.Mesh = modelMesh.Mesh;
            drawbaleElement.Material = modelMesh.Material;
            drawbaleElement.Material.Color = new Color(shading);
            drawbaleElement.Material.CastShadow = true;
            drawbaleElement.Material.ReceiveShadow = useTransparency;
            drawbaleElement.Material.UseTransparency = useTransparency;
            drawbaleElement.Material.IsUnlit = false;
        }
    }
}
