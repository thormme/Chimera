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
            ArmLeft1Cap, ArmLeft2Cap, ArmLeft3Cap,
            ArmRight1Cap, ArmRight2Cap, ArmRight3Cap,

            LegFrontLeft1Cap, LegFrontLeft2Cap, LegFrontLeft3Cap,
            LegFrontRight1Cap, LegFrontRight2Cap, LegFrontRight3Cap,
            LegRearLeft1Cap, LegRearLeft2Cap, LegRearLeft3Cap,
            LegRearRight1Cap, LegRearRight2Cap, LegRearRight3Cap,

            HeadLeftCap, HeadCenterCap, HeadRightCap,

            Spine1Cap, Spine2Cap, Spine3Cap
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

        public abstract int Sneak
        {
            get;
            set;
        }

        public abstract bool Incapacitated
        {
            get;
        }

        public abstract int Intimidation
        {
            get;
            set;
        }

        #endregion

        #region Protected Properties

        protected virtual void OnDeath()
        {
            World.Remove(mSensor);
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

        public Creature(Vector3 position, float height, float radius, float mass, Renderable renderable, RadialSensor radialSensor, Controller controller)
            : base(renderable, new Cylinder(position, height, radius, mass))
        {
            mSensor = radialSensor;
            Forward = new Vector3(0.0f, 0.0f, 1.0f);
            mPartAttachments = new List<PartAttachment>();
            mUnusedPartBones = GetUsablePartBones();

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
                }

                base.World = value;

                if (value != null)
                {
                    value.Add(mSensor);
                    value.Space.Add(CharacterController);
                }
            }
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
                int count = 0;
                foreach (PartBone partBone in partAttachment.Bones)
                {
                    int boneIndex = (mRenderable as AnimateModel).SkinningData.BoneIndices[partBone.ToString()];
                    Matrix worldTransform = (mRenderable as AnimateModel).AnimationPlayer.GetWorldTransforms()[boneIndex] * GetRenderTransform();
                    partAttachment.Part.SubParts[count].Render(worldTransform);

                    count++;
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

            foreach (Part.SubPart subPart in part.SubParts)
            {
                bool foundBone = false;

                // Look for the preferred bones first.
                foreach (PartBone preferredBone in subPart.PreferredBones)
                {
                    if (!foundBone)
                    {
                        if (mUnusedPartBones.Contains(preferredBone) && !partBones.Contains(preferredBone))
                        {
                            partBones.Add(preferredBone);
                            foundBone = true;
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
                    if (!foundBone)
                    {
                        if (!partBones.Contains(unusedBone))
                        {
                            partBones.Add(unusedBone);
                            foundBone = true;
                        }
                    }
                    else
                    {
                        break;
                    }
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

        public override Vector3 Forward
        {
            get
            {
                return base.Forward;
            }
            set
            {
                base.Forward = value;
                //mSensor.Forward = Forward;
            }
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
            if (!Incapacitated)
            {
                CharacterController.Jump();
            }
        }

        /// <summary>
        /// Moves the creature in a direction corresponding to its facing direction.
        /// </summary>
        /// <param name="direction">The direction to move relative to the facing direction.</param>
        public virtual void Move(Vector2 direction)
        {
            if (!Incapacitated)
            {
                CharacterController.HorizontalMotionConstraint.MovementDirection = direction;
                if (direction != Vector2.Zero)
                {
                    Forward = new Vector3(direction.X, 0.0f, direction.Y);
                    mSensor.Forward = Forward;
                }
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
            if (Incapacitated)
            {
                return;
            }

            float elapsedTime = (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            mSensor.Update(gameTime);
            mController.Update(gameTime, mSensor.CollidingCreatures);
            mSensor.Position = Position;
            //mSensor.Forward = Forward;// XNAOrientationMatrix = XNAOrientationMatrix;

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

            foreach (PartAttachment p in mPartAttachments)
            {
                p.Part.Update(gameTime);
            }
        }

        #endregion
    }
}
