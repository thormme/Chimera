using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using BEPUphysics.Entities.Prefabs;
using Microsoft.Xna.Framework;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.Collidables;
using BEPUphysics.Collidables.MobileCollidables;

namespace Chimera
{
    /// <summary>
    /// A box shaped sensor which senses PlayerCreatures.
    /// Calls OnEnter at least once when entered.
    /// Calls OnExit at least once when exited.
    /// </summary>
    public class Trigger : Sensor
    {
        /// <summary>
        /// Constructs a new Trigger.
        /// </summary>
        /// <param name="position">The position of the center of the Trigger.</param>
        /// <param name="orientation">The orientation of the Trigger.</param>
        /// <param name="scale">The scale to modify the Trigger box by.</param>
        public Trigger(Vector3 position, Quaternion orientation, Vector3 scale) :
            base(new Box(position, scale.X, scale.Y, scale.Z))
        {
            Entity.Orientation = orientation;
        }

        /// <summary>
        /// Called at least once when a PlayerCreature enters(touches) the Trigger.
        /// </summary>
        public virtual void OnEnter() {}

        /// <summary>
        /// Called at least once when a PlayerCreature exits(stops touching) the Trigger.
        /// </summary>
        public virtual void OnExit() {}

        public override void InitialCollisionDetected(EntityCollidable sender, Collidable other, CollidablePairHandler collisionPair)
        {
            if (other.Tag is CharacterSynchronizer)
            {
                if ((other.Tag as CharacterSynchronizer).body.Tag is PlayerCreature)
                {
                    OnEnter();
                }
            }
        }

        public override void CollisionEnded(EntityCollidable sender, Collidable other, CollidablePairHandler collisionPair)
        {

            if (other.Tag is CharacterSynchronizer)
            {
                if ((other.Tag as CharacterSynchronizer).body.Tag is PlayerCreature)
                {
                    OnExit();
                }
            }
        }

    }
}
