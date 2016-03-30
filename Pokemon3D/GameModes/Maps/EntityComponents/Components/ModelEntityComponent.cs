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
        private DrawableElement _sceneNode;

        public ModelEntityComponent(Entity parent, EntityRenderModeModel dataModel) : base(parent)
        {
            _dataModel = dataModel;
            InitializeRenderingData(dataModel.IsBillboard, dataModel.RenderMethod, dataModel.PrimitiveModelId, dataModel.Shading.GetVector3(), dataModel.UseTransparency, dataModel.ModelPath);
        }

        public ModelEntityComponent(Entity parent, Mesh mesh, Material material, bool isBillboard) : base(parent)
        {
            _sceneNode = parent.Game.Renderer.CreateDrawableElement(true);
            _sceneNode.Material = material;
            _sceneNode.Mesh = mesh;
            _sceneNode.IsBillboard = isBillboard;
            _sceneNode.EndInitialzing();
            IsBillboard = isBillboard;
        }

        private void InitializeRenderingData(bool isBillboard, RenderMethod renderMethod, string primitiveModelId, Vector3 shadingVector, bool useTransparency, string modelPath)
        {
            _sceneNode = Parent.Game.Renderer.CreateDrawableElement(true);
            _sceneNode.Material = new Material();

            IsBillboard = isBillboard;

            if (renderMethod == RenderMethod.Primitive)
            {
                //todo: that might not be a good idea.
                _sceneNode.Mesh = GameController.Instance.ActiveGameMode.GetPrimitiveMesh(primitiveModelId);
                _sceneNode.Material.Color = new Color(shadingVector);
                _sceneNode.Material.CastShadow = true;
                _sceneNode.Material.ReceiveShadow = !useTransparency;
                _sceneNode.Material.UseTransparency = useTransparency;
                _sceneNode.Material.IsUnlit = false;

                SetTexture(0);
            }
            else
            {
                //todo: that might not be a good idea.
                var models = GameController.Instance.ActiveGameMode.GetModel(modelPath);
                if (models.Length >= 1)
                {
                    AttachModelToSceneNode(_sceneNode, models[0], shadingVector, useTransparency);
                }
            }

            _sceneNode.EndInitialzing();
        }

        public bool IsBillboard
        {
            get { return _sceneNode.IsBillboard; }
            set { _sceneNode.IsBillboard = value; }
        }

        public void SetTexture(int index)
        {
            var texture = _dataModel.Textures[index];

            //todo: that might not be a good idea.
            var diffuseTexture = GameController.Instance.ActiveGameMode.GetTexture(texture.Source);
            _sceneNode.Material.DiffuseTexture = diffuseTexture;

            if (texture.Rectangle != null)
            {
                _sceneNode.Material.TexcoordOffset = diffuseTexture.GetTexcoordsFromPixelCoords(texture.Rectangle.X, texture.Rectangle.Y);
                _sceneNode.Material.TexcoordScale = diffuseTexture.GetTexcoordsFromPixelCoords(texture.Rectangle.Width, texture.Rectangle.Height);
            }
            else
            {
                _sceneNode.Material.TexcoordOffset = Vector2.Zero;
                _sceneNode.Material.TexcoordScale = Vector2.One;
            }
        }

        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);
            _sceneNode.WorldMatrix = Parent.WorldMatrix;

            if (_sceneNode.Mesh != null)
            {
                var box = _sceneNode.Mesh.LocalBounds;
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

                _sceneNode.BoundingBox = box;
                _sceneNode.GlobalPosition = Parent.GlobalPosition;
            }
        }

        public Material Material { get { return _sceneNode.Material; } }

        private void AttachModelToSceneNode(DrawableElement sceneNode, ModelMesh modelMesh, Vector3 shading, bool useTransparency)
        {
            sceneNode.Mesh = modelMesh.Mesh;
            sceneNode.Material = modelMesh.Material;
            sceneNode.Material.Color = new Color(shading);
            sceneNode.Material.CastShadow = true;
            sceneNode.Material.ReceiveShadow = useTransparency;
            sceneNode.Material.UseTransparency = useTransparency;
            sceneNode.Material.IsUnlit = false;
        }
    }
}
