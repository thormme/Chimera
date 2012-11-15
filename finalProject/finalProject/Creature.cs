using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using BEPUphysics.CollisionShapes;
using BEPUphysics.Constraints.TwoEntity.Joints;
using BEPUphysics.Constraints.SingleEntity;
using GameConstructLibrary;
using BEPUphysics.Entities;

namespace finalProject
{
    /// <summary>
    /// Abstract class for a creature. It is controlled by its controller.
    /// </summary>
    abstract public class Creature : Actor
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

        public Creature(Renderable renderable, Entity entity, RadialSensor radialSensor, Controller controller)
            : base(renderable, entity)
        {
            mSensor = radialSensor;
            mController = controller;
            controller.SetCreature(this);
            Game1.World.Add(this);
            MaximumAngularSpeedConstraint constraint = new MaximumAngularSpeedConstraint(Entity, 0.0f);
            // What do I do with this joint?
            throw new NotImplementedException("Creature does not know what to do with the joint.");
        }

        //abstract protected bool OnGround
        //{
        //    get;
        //}

        abstract public float Sneak
        {
            get;
        }

        /// <summary>
        /// Called when the creature is damaged while it has no parts.
        /// </summary>
        abstract protected void OnDeath();

        /// <summary>
        /// Uses the specified part.
        /// </summary>
        /// <param name="part">
        /// The index into the list of parts.
        /// </param>
        /// <param name="direction">
        /// The direction in which to use the part.
        /// </param>
        public virtual void UsePart(int part, Vector3 direction)
        {
            if (part < mParts.Count())
            {
                mParts[part].Use(direction);
            }
        }

        public virtual void Jump()
        {
            //if (OnGround)
            {
                Entity.LinearVelocity = Vector3.Add(JumpVector, Entity.LinearVelocity);
            }
        }

        /// <summary>
        /// Moves the creature in a direction corresponding to its facing direction.
        /// </summary>
        /// <param name="direction">
        /// The direction to move relative to the facing direction.
        /// </param>
        public virtual void Move(Vector2 direction)
        {
            Vector3 forward = Vector3.Multiply(Forward, direction.Y);
            Vector3 left = Vector3.Multiply(Right, direction.X);
            Vector3 temp = Vector3.Add(forward, left);
            Entity.ApplyLinearImpulse(ref temp);
        }

        /// <summary>
        /// Damages the creature by removing parts.
        /// </summary>
        /// <param name="damage">
        /// The amount of damage dealt.
        /// </param>
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

        /// <summary>
        /// Called every frame. 
        /// </summary>
        /// <param name="time">
        /// The game time.
        /// </param>
        public override void Update(GameTime time)
        {
            mController.Update(time, mSensor.CollidingCreatures);
            // Need to set position of sensor
            throw new NotImplementedException("Need to set position of sensor");

            foreach (Part p in mParts)
            {
                p.Update(time);
            }
        }
    }
}
