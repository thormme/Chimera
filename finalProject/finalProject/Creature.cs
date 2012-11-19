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
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;

namespace finalProject
{
    /// <summary>
    /// Abstract class for a creature. It is controlled by its controller.
    /// </summary>
    abstract public class Creature : Actor
    {
        //protected const float MoveAcceleration = 50.0f;
        protected const float DefaultJumpVelocity = 10.0f;
        protected const float DefaultMaxVelocity = 5.0f;

        protected virtual float JumpVelocity
        {
            get
            {
                return DefaultJumpVelocity;
            }
        }

        protected virtual float MaxVelocity
        {
            get
            {
                return DefaultMaxVelocity;
            }
        }

        protected float MaxVelocitySquared
        {
            get
            {
                return MaxVelocity * MaxVelocity;
            }
        }

        protected Vector3 JumpVector
        {
            get
            {
                return new Vector3(0.0f, JumpVelocity, 0.0f);
            }
        }

        protected RadialSensor mSensor;

        protected List<Part> mParts;
        public List<Part> Parts
        {
            get
            {
                return mParts;
            }
        }

        protected Controller mController;
        public Controller CreatureController
        {
            get
            {
                return mController;
            }
        }

        public void AddPart(Part part)
        {
            mParts.Add(part);
            part.Creature = this;
            // STAPLE HERE
        }

        public Creature(Vector3 position, Renderable renderable, Entity entity, RadialSensor radialSensor, Controller controller)
            : base(renderable, entity)
        {
            mSensor = radialSensor;
            Forward = new Vector3(1.0f, 0.0f, 0.0f);
            mController = controller;
            controller.SetCreature(this);
            mParts = new List<Part>();
            Entity.Position = position;
            Game1.World.Add(mSensor);
            //Entity.CollisionInformation.Events.InitialCollisionDetected += InitialCollisionDetected;
            //MaximumAngularSpeedConstraint constraint = new MaximumAngularSpeedConstraint(this, 0.0f);
            // What do I do with this joint?
            //throw new NotImplementedException("Creature does not know what to do with the joint.");
        }

        protected virtual void OnDeath()
        {
            Game1.World.Remove(mSensor);
            foreach (Part cur in mParts)
            {
                Game1.World.Remove(cur);
            }
        }

        protected bool OnGround
        {
            get
            {
                // TODO: check if it is actually on the ground
                return true;
            }
        }

        public abstract float Sneak
        {
            get;
        }

        public abstract bool Incapacitated
        {
            get;
        }

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
            if (OnGround)
            {
                Entity.LinearVelocity += Vector3.Add(JumpVector, Entity.LinearVelocity);
            }
        }

        /// <summary>
        /// Moves the creature in a direction corresponding to its facing direction.
        /// </summary>
        /// <param name="direction">
        /// The direction to move relative to the facing direction.
        /// </param>
        public virtual void Move(float magnitude)
        {
            Entity.LinearVelocity += Vector3.Normalize(Forward) * magnitude * MaxVelocity;
            Vector3 horizVelocity = new Vector3(Entity.LinearVelocity.X, 0.0f, Entity.LinearVelocity.Z);
            if (horizVelocity.LengthSquared() > MaxVelocitySquared)
            {
                horizVelocity.Normalize();
                horizVelocity *= MaxVelocity;
                horizVelocity.Y = Entity.LinearVelocity.Y;
                Entity.LinearVelocity = horizVelocity;
            }
        }

        /// <summary>
        /// Damages the creature by removing parts.
        /// </summary>
        /// <param name="damage">
        /// The amount of damage dealt.
        /// </param>
        public abstract void Damage(int damage);

        /// <summary>
        /// Called every frame. 
        /// </summary>
        /// <param name="time">
        /// The game time.
        /// </param>
        public override void Update(GameTime time)
        {
            Up = new Vector3(0.0f, 1.0f, 0.0f);
            mSensor.Update(time);
            mController.Update(time, mSensor.CollidingCreatures);
            mSensor.Position = Position;
            //if (mRenderable is AnimateModel)
            //{
            //    (mRenderable as AnimateModel).Update(time);
            //}

            foreach (Part p in mParts)
            {
                p.Update(time);
            }
        }
    }
}
