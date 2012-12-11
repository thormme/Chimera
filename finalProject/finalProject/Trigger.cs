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

namespace finalProject
{
    public class Trigger : Sensor
    {

        public Trigger(Vector3 position, float radius) :
            base(new Sphere(position, radius))
        {
        }

        public virtual void OnEnter() {}
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
