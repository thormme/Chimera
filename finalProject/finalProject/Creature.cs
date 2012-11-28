#region Using Statements

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using BEPUphysics.CollisionShapes;
using BEPUphysics.Constraints.TwoEntity.Joints;
using BEPUphysics.Constraints.SingleEntity;
using GameConstructLibrary;
using BEPUphysics.Entities;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.Entities.Prefabs;
using System;

#endregion

namespace finalProject
{
    /// <summary>
    /// Abstract class for a creature. It is controlled by its controller.
    /// </summary>
    abstract public class Creature : Actor
    {
        #region Fields

        protected const float MoveSpeed = 50.0f;
        protected const float JumpVelocity = 10.0f;
        protected Vector3 JumpVector = new Vector3(0.0f, JumpVelocity, 0.0f);

        protected RadialSensor mSensor;
        
        protected List<Part> mParts;
        protected Controller mController;
        
        #endregion

        #region Public Properties

        public Controller CreatureController
        {
            get
            {
                return mController;
            }
        }

        public CharacterController CharacterController
        {
            get;
            private set;
        }

        public List<Part> Parts
        {
            get
            {
                return mParts;
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

        #endregion

        #region Protected Properties

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

        #endregion

        #region Public Methods

        public Creature(Renderable renderable, Entity entity, RadialSensor radialSensor, Controller controller)
            : base(renderable, entity)
        {
            mSensor = radialSensor;
            Forward = new Vector3(0.0f, 0.0f, 1.0f);
            mParts = new List<Part>();
            //Entity.CollisionInformation.Events.InitialCollisionDetected += InitialCollisionDetected;

            CharacterController = new CharacterController(entity, 0.35f);

            mController = controller;
            controller.SetCreature(this);
            //MaximumAngularSpeedConstraint constraint = new MaximumAngularSpeedConstraint(this, 0.0f);
            // What do I do with this joint?
            //throw new NotImplementedException("Creature does not know what to do with the joint.");
        }

        /// <summary>
        /// Attached part to the creature at specified bone.
        /// </summary>
        /// <param name="part"></param>
        public void AddPart(Part part)
        {
            mParts.Add(part);
            part.Creature = this;
            // STAPLE HERE
        }

        /// <summary>
        /// Uses the specified part.
        /// </summary>
        /// <param name="part">The index into the list of parts.</param>
        /// <param name="direction">The direction in which to use the part.</param>
        public virtual void UsePart(int part, Vector3 direction)
        {
            if (part < mParts.Count())
            {
                mParts[part].Use(direction);
            }
        }

        public virtual void Jump()
        {
            CharacterController.Jump();
        }

        /// <summary>
        /// Moves the creature in a direction corresponding to its facing direction.
        /// </summary>
        /// <param name="direction">The direction to move relative to the facing direction.</param>
        public virtual void Move(Vector2 direction)
        {
            CharacterController.HorizontalMotionConstraint.MovementDirection = direction;
            if (direction != Vector2.Zero)
            {
                Forward = new Vector3(direction.X, 0.0f, direction.Y);
            }
        }

        /// <summary>
        /// Damages the creature by removing parts.
        /// </summary>
        /// <param name="damage">The amount of damage dealt.</param>
        public abstract void Damage(int damage);

        /// <summary>
        /// Called every frame. 
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            //Up = new Vector3(0.0f, 1.0f, 0.0f);
            mSensor.Update(gameTime);
            mController.Update(gameTime, mSensor.CollidingCreatures);
            mSensor.Position = Position;

            List<BEPUphysics.RayCastResult> results = new List<BEPUphysics.RayCastResult>();
            Game1.World.mSpace.RayCast(new Ray(Position, -1.0f * Up), 4.0f, results);

            BEPUphysics.RayCastResult result = new BEPUphysics.RayCastResult();
            foreach (BEPUphysics.RayCastResult collider in results)
            {
                if (collider.HitObject as Collidable != CharacterController.Body.CollisionInformation)
                {
                    result = collider;
                    break;
                }
            }

            foreach (Part p in mParts)
            {
                p.Update(gameTime);
            }
        }

        #endregion
    }
}
