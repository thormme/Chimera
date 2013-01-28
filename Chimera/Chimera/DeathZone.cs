using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;

namespace Chimera
{
    /// <summary>
    /// Kills Creatures when touched.
    /// </summary>
    public class DeathZone : Sensor
    {
        /// <summary>
        /// Constructs a new DeathZone.
        /// </summary>
        /// <param name="position">The center of the DeathZone.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="length">The length.</param>
        public DeathZone(Vector3 position, float width, float height, float length)
            : base(CreateEntity(position, width, height, length))
        {
            Entity.Tag = this;
            Entity.CollisionInformation.Tag = this;
        }

        private static Entity CreateEntity(Vector3 position, float width, float height, float length)
        {
            Entity entity = new Box(position, width, height, length);
            return entity;
        }

        public override void InitialCollisionDetected(EntityCollidable sender, Collidable other, CollidablePairHandler collisionPair)
        {
            if (other.Tag is CharacterSynchronizer)
            {
                if ((other.Tag as CharacterSynchronizer).body.Tag is Creature)
                {
                    ((other.Tag as CharacterSynchronizer).body.Tag as Creature).Die();
                }
            }
        }
    }
}
