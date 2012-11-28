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

        protected List<Part> mParts;
        protected Controller mController;
        protected RadialSensor mSensor;
        
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

        public Creature(Vector3 position, float height, float radius, float mass, Renderable renderable, RadialSensor radialSensor, Controller controller)
            : base(renderable, new Cylinder(position, height, radius, mass))
        {
            mSensor = radialSensor;
            Forward = new Vector3(0.0f, 0.0f, 1.0f);
            mParts = new List<Part>();
            
            CharacterController = new CharacterController(Entity, 1.0f);

            mController = controller;
            controller.SetCreature(this);
        }

        public override World World
        {
            get
            {
                return base.World;
            }
            set
            {
                if (World != null)
                {
                    World.Remove(mSensor);
                    World.Space.Remove(CharacterController);

                    foreach (Part cur in mParts)
                    {
                        World.Remove(cur);
                    }
                }

                base.World = value;

                if (value != null)
                {
                    value.Add(mSensor);
                    value.Space.Add(CharacterController);

                    foreach (Part part in mParts)
                    {
                        World.Add(part);
                    }
                }
            }
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
        /// Damages the creature.
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

            mSensor.Update(gameTime);
            mController.Update(gameTime, mSensor.CollidingCreatures);
            mSensor.Position = Position;

            List<BEPUphysics.RayCastResult> results = new List<BEPUphysics.RayCastResult>();
            World.Space.RayCast(new Ray(Position, -1.0f * Up), 4.0f, results);

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
