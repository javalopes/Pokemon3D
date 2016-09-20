using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Animations;
using Pokemon3D.Common.Extensions;
using Pokemon3D.DataModel.GameMode.Map.NPCs;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.Entities
{
    //class NPC : Entity
    //{
    //    private string _name;
    //    private NPCBehaviour _behaviour;
    //    private string _interactionScriptBinding;
    //    private Animator _figureAnimator;

    //    public NPC(Scene scene, RandomNPCModel dataModel)
    //    {
    //        _name = dataModel.Name;
    //        _behaviour = dataModel.Behaviour;
    //        _interactionScriptBinding = dataModel.ScriptBinding;

    //        SceneNode.Mesh = new Mesh(Game.GraphicsDevice, Primitives.GenerateQuadForYBillboard());
    //        var diffuseTexture = Game.Content.Load<Texture2D>(ResourceNames.Windows.Textures.DefaultGuy);
    //        SceneNode.Material = new Material
    //        {
    //            DiffuseTexture = diffuseTexture,
    //            UseTransparency = true,
    //            TexcoordScale = diffuseTexture.GetTexcoordsFromPixelCoords(32, 32),
    //            IsUnlit = true
    //        };
    //        SceneNode.Position = new Vector3(1, 1, 8);
    //        SceneNode.IsBillboard = true;

    //        SceneNode.EndInitializing();

    //        _figureAnimator = new Animator();
    //        _figureAnimator.AddAnimation("WalkForward", Animation.CreateDiscrete(0.65f, new[]
    //        {
    //            diffuseTexture.GetTexcoordsFromPixelCoords(0, 0),
    //            diffuseTexture.GetTexcoordsFromPixelCoords(32, 0),
    //            diffuseTexture.GetTexcoordsFromPixelCoords(0, 0),
    //            diffuseTexture.GetTexcoordsFromPixelCoords(64, 0),
    //        }, t => SceneNode.Material.TexcoordOffset = t, true));
    //        _figureAnimator.AddAnimation("WalkLeft", Animation.CreateDiscrete(0.65f, new[]
    //        {
    //            diffuseTexture.GetTexcoordsFromPixelCoords(0, 32),
    //            diffuseTexture.GetTexcoordsFromPixelCoords(32, 32),
    //            diffuseTexture.GetTexcoordsFromPixelCoords(0, 32),
    //            diffuseTexture.GetTexcoordsFromPixelCoords(64, 32),
    //        }, t => SceneNode.Material.TexcoordOffset = t, true));
    //        _figureAnimator.AddAnimation("WalkRight", Animation.CreateDiscrete(0.65f, new[]
    //        {
    //            diffuseTexture.GetTexcoordsFromPixelCoords(0, 96),
    //            diffuseTexture.GetTexcoordsFromPixelCoords(32, 96),
    //            diffuseTexture.GetTexcoordsFromPixelCoords(0, 96),
    //            diffuseTexture.GetTexcoordsFromPixelCoords(64, 96),
    //        }, t => SceneNode.Material.TexcoordOffset = t, true));
    //        _figureAnimator.AddAnimation("WalkBackward", Animation.CreateDiscrete(0.65f, new[]
    //        {
    //            diffuseTexture.GetTexcoordsFromPixelCoords(0, 64),
    //            diffuseTexture.GetTexcoordsFromPixelCoords(32, 64),
    //            diffuseTexture.GetTexcoordsFromPixelCoords(0, 64),
    //            diffuseTexture.GetTexcoordsFromPixelCoords(64, 64),
    //        }, t => SceneNode.Material.TexcoordOffset = t, true));

    //        Collider = Collisions.Collider.CreateBoundingBox(new Vector3(0.35f, 0.6f, 0.35f), new Vector3(0.0f, 0.3f, 0.0f));
    //    }

    //    public override void Update(float elapsedTime)
    //    {
    //        base.Update(elapsedTime);

    //        _figureAnimator.Update(elapsedTime);

    //        Collider.SetPosition(SceneNode.Position);

    //        var collidingObjects = Game.CollisionManager.CheckCollision(Collider);
    //        if (collidingObjects.Length > 0)
    //        {
    //            for (var i = 0; i < collidingObjects.Length; i++)
    //            {
    //                SceneNode.Position = SceneNode.Position += collidingObjects[i].Axis;
    //            }
    //            Collider.SetPosition(SceneNode.Position);
    //        }
    //    }
    //}
}
