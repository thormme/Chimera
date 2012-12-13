using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using GraphicsLibrary;
using BEPUphysics.Entities;
using Microsoft.Xna.Framework;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.BroadPhaseSystems;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Entities.Prefabs;
using finalProject.Menus;

namespace finalProject
{
    /// <summary>
    /// The goal for that the player must reach.
    /// </summary>
    public class GoalPoint : PhysicsObject
    {
        string mNextLevel = null;
        bool mLoaded = false;
        public Type PartType;

        public GoalPoint(Vector3 position, Quaternion orientation, Vector3 scale, string nextLevel, Type partType)
            : base(new ScrollingTransparentModel("tractorBeam"), new Cylinder(position, 1f, scale.Length()))
        {
            mNextLevel = nextLevel;
            PartType = partType;
            Scale = new Vector3(1.0f, 5.0f, 1.0f);
            (mRenderable as ScrollingTransparentModel).HorizontalVelocity = 0.1f;
            (mRenderable as ScrollingTransparentModel).VerticalVelocity = 0.0f;
        }

        /// <summary>
        /// Constructor for use by the World level loading.
        /// </summary>
        /// <param name="modelName">Name of the model.</param>
        /// <param name="translation">The position.</param>
        /// <param name="orientation">The orientation.</param>
        /// <param name="scale">The amount to scale by.</param>
        /// <param name="extraParameters">Extra parameters.</param>
        public GoalPoint(String modelName, Vector3 translation, Quaternion orientation, Vector3 scale, string[] extraParameters)
            : this(translation, orientation, scale, extraParameters[0], Type.GetType("finalProject.Parts." + extraParameters[1] + ", finalProject"))
        {
        }

        public override void InitialCollisionDetected(EntityCollidable sender, Collidable other, CollidablePairHandler collisionPair)
        {
            if (other.Tag is CharacterSynchronizer && !mLoaded)
            {
                CharacterSynchronizer synchronizer = (other.Tag as CharacterSynchronizer);
                if (synchronizer.body.Tag is PlayerCreature)
                {
                    // Check that the player has the requested part.
                    bool hasCorrectPart = false;
                    foreach (Creature.PartAttachment partAttachment in (synchronizer.body.Tag as Creature).PartAttachments)
                    {
                        if (partAttachment != null && partAttachment.Part.GetType() == PartType)
                        {
                            hasCorrectPart = true;
                            break;
                        }
                    }
                    if (hasCorrectPart)
                    {
                        // Load the next level.
                        mLoaded = true;
                        // TODO: Investigate last frame object additions to next world.
                        World.Clear();
                        // TODO: Make scale globally accessible or modifiable.
                        if (LevelManager.Exists(mNextLevel))
                        {
                            World.AddLevelFromFile(mNextLevel, Vector3.Zero, Quaternion.Identity, new Vector3(8.0f, 0.01f, 8.0f));
                        }
                        else
                        {
                            InputAction.IsMouseLocked = false;
                            Game1.PopState();
                            Game1.PushState(new SuccessMenu(Game1.Game));
                        }
                    }
                }
            }
        }
    }
}