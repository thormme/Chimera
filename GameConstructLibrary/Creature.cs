using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary
{
    abstract public class Creature : MyActor
    {
        protected static float JumpVelocity = 50.0f;
        protected Vector3 JumpVector = new Vector3(0.0f, JumpVelocity, 0.0f);

        protected RadialSensor mSensor;
        protected List<Part> mParts;

        protected Controller mController;
        public Controller CreatureController
        {
            get
            {
                return mController;
            }
        }

        public Creature()
        {
            // TODO: attach radial sensor
        }

        abstract protected bool OnGround
        {
            get;
        }

        abstract public float Sneak
        {
            get;
        }

        abstract protected void OnDeath();

        public virtual void UsePart(int part, Vector3 direction)
        {
            mParts[part].Use(direction);
        }

        public virtual void Jump()
        {
            if (OnGround)
            {
                Velocity = Vector3.Add(JumpVector, Velocity);
            }
        }

        public virtual void Move(Vector2 direction)
        {
            Vector3 forward = Vector3.Multiply(Forward, direction.Y);
            Vector3 left = Vector3.Multiply(Left, direction.X);
            Acceleration = Vector3.Add(forward, left);
        }

        public virtual void Damage(int damage)
        {
            while (damage-- > 0)
            {
                if (mParts.Count() == 0)
                {
                    OnDeath();
                    return;
                }

                mParts.Remove(mParts[Rand.rand.Next(mParts.Count())]);
            }
        }

        public override void Update(GameTime time)
        {
            mController.Update(time, mSensor.CollidingCreatures);

            foreach (Part p in mParts)
            {
                p.Update(time);
            }
        }
    }
}
