using Microsoft.Xna.Framework;
using Pokemon3D.Common;
using Pokemon3D.Rendering.Data;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Pokemon3D.Rendering
{
    /// <summary>
    /// Representing a whole 3D Scene with all objects to display.
    /// </summary>
    public class Scene : GameContextObject
    {
        private List<SceneNode> _initializingNodes = new List<SceneNode>();

        internal readonly object LockObject = new object();

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
            Light = new Light();
        }

        public void OnViewSizeChanged(Rectangle oldSize, Rectangle newSize)
        {
            foreach(var camera in AllCameras)
            {
                camera.OnViewSizeChanged(oldSize, newSize);
            }
        }

        /// <summary>
        /// Creates a new sceneNode instance.
        /// </summary>
        /// <param name="markAsInitializing">Set this to true to edit your sceneNode preventing it from processing during initialization in multithreaded environments.</param>
        /// <returns></returns>
        public SceneNode CreateSceneNode(bool markAsInitializing = false)
        {
            var sceneNode = new SceneNode(markAsInitializing, OnInitializationFinished);
            HasSceneNodesChanged = true;
            lock (LockObject)
            {
                if (markAsInitializing)
                {
                    _initializingNodes.Add(sceneNode);
                }
                else
                {
                    AllSceneNodes.Add(sceneNode);
                }
            }
            return sceneNode;
        }

        private void OnInitializationFinished(SceneNode sceneNode)
        {
            lock (LockObject)
            {
                _initializingNodes.Remove(sceneNode);
                AllSceneNodes.Add(sceneNode);
            }
        }

        /// <summary>
        /// Removes Scenenode from scene.
        /// </summary>
        /// <param name="node">scene node</param>
        public void RemoveSceneNode(SceneNode node)
        {
            HasSceneNodesChanged = true;

            lock (LockObject)
            {
                AllSceneNodes.Remove(node);
            }
        }

        /// <summary>
        /// Creates a new camera with default viewport of screen.
        /// </summary>
        /// <returns>Camera</returns>
        public Camera CreateCamera()
        {
            HasSceneNodesChanged = true;
            var camera = new Camera(GameContext.GraphicsDevice.Viewport);

            lock (LockObject)
            {
                AllCameras.Add(camera);
            }
            
            return camera;
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

            lock (LockObject)
            {
                AllSceneNodes.Add(cloned);
            }

            return cloned;
        }
    }
}
