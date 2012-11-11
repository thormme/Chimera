using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics.CollisionShapes.ConvexShapes;

namespace GameConstructLibrary
{
    /// <summary>
    /// The creature representing the player character.
    /// </summary>
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

        /// <summary>
        /// Called when the creature is damage while it has no parts.
        /// </summary>
        protected override void OnDeath()
        {
            // TODO: respawn
        }
        
        /// <summary>
        /// Adds a part to the PlayerCreature. The part chosen is the closest part within the radial sensor.
        /// </summary>
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
