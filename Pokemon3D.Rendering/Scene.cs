using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Pokemon3D.Common;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.Rendering
{
    /// <summary>
    /// Representing a whole 3D Scene with all objects to display.
    /// </summary>
    public class Scene : GameContextObject
    {
        /// <summary>
        /// Ambient Light for all Objects. Default is white.
        /// </summary>
        public Vector4 AmbientLight { get; set; }
        
        /// <summary>
        /// Currently single light just supported.
        /// </summary>
        public Light Light { get; set; }

        internal bool HasSceneNodesChanged { get; set; }
        internal List<SceneNode> AllSceneNodes { get; }
        internal List<Camera> AllCameras { get; }
        internal List<SceneNode> StaticNodes { get; }

        /// <summary>
        /// Creates a new scene for rendering objects.
        /// </summary>
        /// <param name="context">Game Context.</param>
        public Scene(GameContext context) : base(context)
        {
            AllSceneNodes = new List<SceneNode>();
            AllCameras = new List<Camera>();
            StaticNodes = new List<SceneNode>();
            AmbientLight = new Vector4(1,1,1,1);
            Light = new Light(GameContext.GraphicsDevice);
        }

        /// <summary>
        /// Creates a new sceneNode instance.
        /// </summary>
        /// <returns></returns>
        public SceneNode CreateSceneNode()
        {
            HasSceneNodesChanged = true;
            var sceneNode = new SceneNode();
            AllSceneNodes.Add(sceneNode);
            return sceneNode;
        }

        /// <summary>
        /// Creates a static mesh which can be merged.
        /// </summary>
        /// <param name="rotation"></param>
        /// <param name="material"></param>
        /// <param name="mesh"></param>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        public void CreateStaticSceneNode(Vector3 position, Vector3 scale, Vector3 rotation, Material material, Mesh mesh)
        {
            var staticNode = new SceneNode
            {
                Position = position,
                Scale = scale,
                Material = material,
                Mesh = mesh
            };
            staticNode.RotateX(rotation.X);
            staticNode.RotateY(rotation.Y);
            staticNode.RotateZ(rotation.Z);
            staticNode.Update();

            StaticNodes.Add(staticNode);
        }

        /// <summary>
        /// Removes Scenenode from scene.
        /// </summary>
        /// <param name="node">scene node</param>
        public void RemoveSceneNode(SceneNode node)
        { 
            HasSceneNodesChanged = true;
            AllSceneNodes.Remove(node);
            AllCameras.Remove(node as Camera);
        }

        /// <summary>
        /// Creates a new camera with default viewport of screen.
        /// </summary>
        /// <returns>Camera</returns>
        public Camera CreateCamera()
        {
            HasSceneNodesChanged = true;
            var camera = new Camera(GameContext.GraphicsDevice.Viewport);
            AllCameras.Add(camera);
            AllSceneNodes.Add(camera);
            return camera;
        }

        /// <summary>
        /// Updates all sceneNodes.
        /// </summary>
        /// <param name="elapsedTime">elapsed time since last call.</param>
        public void Update(float elapsedTime)
        {
            foreach (var sceneNode in AllSceneNodes.OrderBy(n => n.Parent != null))
            {
                sceneNode.Update();
            }
        }

        /// <summary>
        /// Clones a Scene node with its children and all attached Properties.
        /// Meshes will only be cloned when <see cref="cloneMeshs"/> is true.
        /// </summary>
        /// <param name="nodeToClone">Node to clone</param>
        /// <param name="cloneMeshs">Force cloning mesh data.</param>
        /// <returns>SceneNode cloned.</returns>
        public SceneNode CloneNode(SceneNode nodeToClone, bool cloneMeshs = false)
        {
            HasSceneNodesChanged = true;
            var cloned = nodeToClone.Clone(cloneMeshs);
            AllSceneNodes.Add(cloned);
            CloneChildren(cloned, nodeToClone, cloneMeshs);
            return cloned;
        }

        private void CloneChildren(SceneNode parentCloned, SceneNode parentOriginal, bool cloneMeshs)
        {
            foreach (var childNode in parentOriginal.Children)
            {
                var clonedChild = childNode.Clone(cloneMeshs);
                AllSceneNodes.Add(clonedChild);
                parentCloned.AddChild(clonedChild);

                CloneChildren(clonedChild, childNode, cloneMeshs);
            }
        }
    }
}
