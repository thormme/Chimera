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

namespace finalProject
{
    /// <summary>
    /// Abstract class for a creature. It is controlled by its controller.
    /// </summary>
    abstract public class Creature : Actor
    {
        protected const float MoveSpeed = 10.0f;
        protected const float JumpVelocity = 10.0f;
        protected Vector3 JumpVector = new Vector3(0.0f, JumpVelocity, 0.0f);

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

        private CompoundCollidable GetCompoundCollidable(PhysicsObject physicsObject)
        {
            List<CompoundChildData> list = new List<CompoundChildData>();
            CompoundChildData data1 = new CompoundChildData();
            CompoundChildData data2 = new CompoundChildData();
            data1.Entry = new CompoundShapeEntry(Entity.CollisionInformation.Shape);
            data1.Tag = this;
            data2.Entry = new CompoundShapeEntry(physicsObject.Entity.CollisionInformation.Shape);
            data2.Tag = physicsObject;

            list.Add(data1);
            list.Add(data2);

            return new CompoundCollidable(list);
        }

        protected void AddPart(Part part)
        {
            (Entity as MorphableEntity).SetCollisionInformation(GetCompoundCollidable(part));
            mParts.Add(part);
        }

        public Creature(Vector3 position, Renderable renderable, Entity entity, RadialSensor radialSensor, Controller controller)
            : base(renderable, entity)
        {
            Entity = new MorphableEntity(GetCompoundCollidable(radialSensor));
            Entity.Tag = this;
            Entity.Position = position;
            Forward = new Vector3(1.0f, 0.0f, 0.0f);
            mSensor = radialSensor;
            mController = controller;
            controller.SetCreature(this);
            mParts = new List<Part>();
            //MaximumAngularSpeedConstraint constraint = new MaximumAngularSpeedConstraint(this, 0.0f);
            // What do I do with this joint?
            //throw new NotImplementedException("Creature does not know what to do with the joint.");
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
        public virtual void Move(Vector2 direction)
        {
            System.Console.WriteLine("direction = x:" + direction.X + " y:" + direction.Y);
            Vector3 forward = Vector3.Multiply(Forward, direction.Y * MoveSpeed);
            Vector3 right = Vector3.Multiply(Right, direction.X * MoveSpeed);
            Vector3 temp = Vector3.Add(forward, right);
            System.Console.WriteLine("temp = x:" + temp.X + " y:" + temp.Y + " z:" + temp.Z);
            Entity.LinearVelocity += temp;
            //PhysicsEntity.ApplyLinearImpulse(ref temp);
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
