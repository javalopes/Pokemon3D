using System.Collections.Generic;
using System.Linq;
using Pokemon3D.Common;

namespace Pokemon3D.Rendering
{
    /// <summary>
    /// Representing a whole 3D Scene with all objects to display.
    /// </summary>
    public class Scene : GameContextObject
    {
        internal bool HasSceneNodesChanged { get; set; }

        internal List<SceneNode> AllSceneNodes { get; }
        internal List<Camera> AllCameras { get; }

        public Scene(GameContext context) : base(context)
        {
            AllSceneNodes = new List<SceneNode>();
            AllCameras = new List<Camera>();
        }

        public SceneNode CreateSceneNode()
        {
            HasSceneNodesChanged = true;
            var sceneNode = new SceneNode();
            AllSceneNodes.Add(sceneNode);
            return sceneNode;
        }
        
        public void RemoveSceneNode(SceneNode node)
        { 
            HasSceneNodesChanged = true;
            AllSceneNodes.Remove(node);
            AllCameras.Remove(node as Camera);
        }

        public Camera CreateCamera()
        {
            HasSceneNodesChanged = true;
            var camera = new Camera(GameContext.GraphicsDevice.Viewport);
            AllCameras.Add(camera);
            AllSceneNodes.Add(camera);
            return camera;
        }

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
