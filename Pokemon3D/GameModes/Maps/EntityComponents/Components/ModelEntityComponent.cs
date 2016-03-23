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
        private SceneNode _sceneNode;

        public SceneNode SceneNode { get { return _sceneNode; } }

        public ModelEntityComponent(Entity parent, EntityRenderModeModel dataModel) : base(parent)
        {
            _dataModel = dataModel;
            InitializeRenderingData(dataModel.IsBillboard, dataModel.RenderMethod, dataModel.PrimitiveModelId, dataModel.Shading.GetVector3(), dataModel.UseTransparency, dataModel.ModelPath);
        }

        public ModelEntityComponent(Entity parent, Mesh mesh, Material material, bool isBillboard) : base(parent)
        {
            _sceneNode = Parent.Scene.CreateSceneNode(true);
            _sceneNode.Material = material;
            _sceneNode.Mesh = mesh;
            IsBillboard = isBillboard;
            _sceneNode.EndInitializing();
        }

        private void InitializeRenderingData(bool isBillboard, RenderMethod renderMethod, string primitiveModelId, Vector3 shadingVector, bool useTransparency, string modelPath)
        {
            _sceneNode = Parent.Scene.CreateSceneNode(true);
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

                if (models.Length == 1)
                {
                    AttachModelToSceneNode(_sceneNode, models[0], shadingVector, useTransparency);
                }
                else
                {
                    foreach (var modelMesh in models)
                    {
                        var childNode = Parent.Scene.CreateSceneNode(true);
                        AttachModelToSceneNode(childNode, modelMesh, shadingVector, useTransparency);
                        _sceneNode.AddChild(childNode);
                        childNode.EndInitializing();
                    }
                }
            }

            if (Parent.IsStatic)
            {
                Parent.Scene.ConvertToStaticSceneNode(_sceneNode);
            }

            _sceneNode.EndInitializing();
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

        private void AttachModelToSceneNode(SceneNode sceneNode, ModelMesh modelMesh, Vector3 shading, bool useTransparency)
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
