using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics.CollisionShapes.ConvexShapes;

namespace GameConstructLibrary
{
    class PlayerCreature : Creature
    {
        private const float mPlayerRadius = 50.0f;

        private const float mSneak = 10.0f;
        public override float Sneak
        {
            get
            {
                return mSneak;
            }
        }

        PlayerCreature()
            : base(null, new SphereShape(mPlayerRadius), new RadialSensor(100.0f), new PlayerController())
        {}

        protected override void OnDeath()
        {
            // TODO: respawn
        }

        public void AddPart()
        {
            foreach (PhysicsObject obj in mSensor.CollidingProps)
            {
                if (obj as Part != null)
                {
                    mParts.Add(obj as Part);
                    return;
                }
            }
        }
    }
}
