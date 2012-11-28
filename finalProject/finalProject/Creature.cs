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
        /// <summary>
        /// Bone names which may be available for attaching parts.
        /// </summary>
        public enum PartBone
        {
            ArmLeft1, ArmLeft2, ArmLeft3,
            ArmRight1, ArmRight2, ArmRight3,

            LegFrontLeft1, LegFrontLeft2, LegFrontLeft3,
            LegFrontRight1, LegFrontRight2, LegFrontRight3,
            LegRearLeft1, LegRearLeft2, LegRearLeft3,
            LegRearRight1, LegRearRight2, LegRearRight3,

            HeadLeft, HeadCenter, HeadRight,

            Spine1, Spine2, Spine3,

            L_Index1
        }

        #region Fields
        public class PartAttachment
        {
            public Part Part;
            public List<PartBone> Bones;

            public PartAttachment(Part part, List<PartBone> bones)
            {
                Part = part;
                Bones = bones;
            }
        }

        protected const float MoveSpeed = 50.0f;
        protected const float JumpVelocity = 10.0f;
        protected Vector3 JumpVector = new Vector3(0.0f, JumpVelocity, 0.0f);

        protected RadialSensor mSensor;

        protected List<PartAttachment> mPartAttachments;
        protected List<PartBone> mUnusedPartBones;

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

        public List<PartAttachment> PartAttachments
        {
            get
            {
                return mPartAttachments;
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
            mPartAttachments.Clear();
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
            mPartAttachments = new List<PartAttachment>();
            mUnusedPartBones = GetUsablePartBones();
            //Entity.CollisionInformation.Events.InitialCollisionDetected += InitialCollisionDetected;

            CharacterController = new CharacterController(entity, 1.0f);

            mController = controller;
            controller.SetCreature(this);
            //MaximumAngularSpeedConstraint constraint = new MaximumAngularSpeedConstraint(this, 0.0f);
            // What do I do with this joint?
            //throw new NotImplementedException("Creature does not know what to do with the joint.");
        }

        protected virtual Matrix GetRenderTransform()
        {
            return Entity.WorldTransform * Matrix.CreateScale(Scale);
        }

        public override void Render()
        {
            if (mRenderable != null)
            {
                mRenderable.Render(GetRenderTransform());
            }
            RenderParts();
        }

        protected void RenderParts()
        {
            foreach (PartAttachment partAttachment in mPartAttachments)
            {
                foreach (PartBone partBone in partAttachment.Bones)
                {
                    int boneIndex = (mRenderable as AnimateModel).SkinningData.BoneIndices[partBone.ToString()];
                    Matrix worldTransform = (mRenderable as AnimateModel).AnimationPlayer.GetWorldTransforms()[boneIndex] * GetRenderTransform();
                    partAttachment.Part.Render(worldTransform);
                }
            }
        }

        protected abstract List<PartBone> GetUsablePartBones();

        /// <summary>
        /// Gets a list of bones to be used to connect a part.
        /// </summary>
        /// <param name="part">The part to fetch bones for.</param>
        /// <returns>List of bones for use by the part.</returns>
        private List<PartBone> GetPartBonesForPart(Part part)
        {
            List<PartBone> partBones = new List<PartBone>();
            // Look for the preferred bones first.
            foreach (PartBone preferredBone in part.PreferredBones)
            {
                if (partBones.Count < part.LimbCount)
                {
                    if (mUnusedPartBones.Contains(preferredBone) && !partBones.Contains(preferredBone))
                    {
                        partBones.Add(preferredBone);
                    }
                }
                else
                {
                    break;
                }
            }
            // If too few preferred bones were available choose any free bone.
            foreach (PartBone unusedBone in mUnusedPartBones)
            {
                if (partBones.Count < part.LimbCount)
                {
                    if (!partBones.Contains(unusedBone))
                    {
                        partBones.Add(unusedBone);
                    }
                }
                else
                {
                    break;
                }
            }

            return partBones;
        }

        /// <summary>
        /// Attach part to the creature.
        /// </summary>
        /// <param name="part">The part to attach.</param>
        public void AddPart(Part part)
        {
            List<PartBone> usedBones = GetPartBonesForPart(part);
            foreach (PartBone partBone in usedBones)
            {
                mUnusedPartBones.Remove(partBone);
            }

            PartAttachment attachment = new PartAttachment(part, usedBones);

            mPartAttachments.Add(attachment);
            part.Creature = this;
        }

        /// <summary>
        /// Remove attached part from the creature.
        /// </summary>
        /// <param name="part"></param>
        public void RemovePart(Part part)
        {
            PartAttachment partAttachment = null;
            foreach (PartAttachment attachment in mPartAttachments)
            {
                if (part == attachment.Part)
                {
                    partAttachment = attachment;
                    break;
                }
            }

            if (partAttachment == null)
            {
                throw new Exception("Could not remove part as it is not owned by the creature");
            }

            foreach (PartBone partBone in partAttachment.Bones)
            {
                mUnusedPartBones.Add(partBone);
            }

            mPartAttachments.Remove(partAttachment);
            partAttachment.Part.Creature = null;
        }

        /// <summary>
        /// Uses the specified part.
        /// </summary>
        /// <param name="part">The index into the list of parts.</param>
        /// <param name="direction">The direction in which to use the part.</param>
        public virtual void UsePart(int part, Vector3 direction)
        {
            if (part < mPartAttachments.Count())
            {
                mPartAttachments[part].Part.Use(direction);
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

            foreach (PartAttachment p in mPartAttachments)
            {
                p.Part.Update(gameTime);
            }
        }

        #endregion
    }
}
