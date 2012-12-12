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

namespace finalProject
{
    public class Checkpoint : PhysicsObject
    {

        public Checkpoint(Vector3 position, Quaternion orientation, Vector3 scale)
            : base(new InanimateModel("box"), new Cylinder(position, 1f, scale.Length()))
        { }

        /// <summary>
        /// Constructor for use by the World level loading.
        /// </summary>
        /// <param name="modelName">Name of the model.</param>
        /// <param name="translation">The position.</param>
        /// <param name="orientation">The orientation.</param>
        /// <param name="scale">The amount to scale by.</param>
        /// <param name="extraParameters">Extra parameters.</param>
        public Checkpoint(String modelName, Vector3 translation, Quaternion orientation, Vector3 scale, string[] extraParameters)
            : this(translation, orientation, scale)
        {
        }

        public override void InitialCollisionDetected(EntityCollidable sender, Collidable other, CollidablePairHandler collisionPair)
        {
            if (other.Tag is CharacterSynchronizer)
            {
                CharacterSynchronizer synchronizer = (other.Tag as CharacterSynchronizer);
                if (synchronizer.body.Tag is PlayerCreature)
                {
                    (synchronizer.body.Tag as PlayerCreature).SpawnOrigin = Position;
                }
            }
        }
    }
}
