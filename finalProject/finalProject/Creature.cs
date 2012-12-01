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
using System.IO;

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

        protected const double InvulnerableLength = 1.0f;

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

        protected List<PartAttachment> mPartAttachments;
        protected List<PartBone> mUnusedPartBones;

        protected int mInvulnerableCount = 0;
        protected double mInvulnerableTimer = -1.0f;
        
        #endregion

        #region Public Properties

        public Controller Controller
        {
            get;
            set;
        }

        public RadialSensor Sensor
        {
            get;
            set;
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

        public bool Invulnerable
        {
            get
            {
                return mInvulnerableCount > 0;
            }
            set
            {
                if (value)
                {
                    ++mInvulnerableCount;
                }
                else if (Invulnerable)
                {
                    --mInvulnerableCount;
                }
            }
        }

        private float mSlideSlope;
        public float SlideSlope
        {
            get
            {
                return mSlideSlope;
            }
            protected set
            {
                mSlideSlope = value;
                CharacterController.SupportFinder.MaximumSlope = value;
            }
        }

        #endregion

        #region Bone Transform Tweaking

        private const int mNumParts = 24;

        /// <summary>
        /// Orientation of different part rotations.
        /// </summary>
        public Matrix BoneRotations
        {
            get
            {
                return mPartRotations[mBoneIndex];
            }
            set
            {
                mPartRotations[mBoneIndex] = value;
            }
        }
        private Matrix[] mPartRotations = new Matrix[mNumParts];

        /// <summary>
        /// Up Basis Vector for current part rotation.
        /// </summary>
        public Vector3 BoneUp
        {
            get
            {
                return mBoneUp[mBoneIndex];
            }
            set
            {
                mBoneUp[mBoneIndex] = value;
            }
        }
        private Vector3[] mBoneUp = new Vector3[mNumParts];

        /// <summary>
        /// Forward Basis Vector for current part rotation.
        /// </summary>
        public Vector3 BoneForward
        {
            get
            {
                return mBoneForward[mBoneIndex];
            }
            set
            {
                mBoneForward[mBoneIndex] = value;
            }
        }
        private Vector3[] mBoneForward = new Vector3[mNumParts];

        /// <summary>
        /// Right Basis Vector for current part rotation.
        /// </summary>
        public Vector3 BoneRight
        {
            get
            {
                return mBoneRight[mBoneIndex];
            }
            set
            {
                mBoneRight[mBoneIndex] = value;
            }
        }
        private Vector3[] mBoneRight = new Vector3[mNumParts];

        public int BoneIndex
        {
            get
            {
                return mBoneIndex;
            }
            set
            {
                mBoneIndex = value;
                if (mBoneIndex < 0)
                {
                    mBoneIndex += mNumParts;
                }
                mBoneIndex %= mNumParts;
            }
        }
        private int mBoneIndex = 0;

        public void WriteBoneTransforms()
        {
            TextWriter tw = new StreamWriter("playerBeanPartOrientations.txt");

            tw.WriteLine(mNumParts);
            for (int i = 0; i < mNumParts; ++i)
            {
                tw.WriteLine(((PartBone)i).ToString());
                tw.WriteLine(mPartRotations[i].M11.ToString() + " " + mPartRotations[i].M12.ToString() + " " + mPartRotations[i].M13.ToString() + " " + mPartRotations[i].M14.ToString());
                tw.WriteLine(mPartRotations[i].M21.ToString() + " " + mPartRotations[i].M22.ToString() + " " + mPartRotations[i].M23.ToString() + " " + mPartRotations[i].M24.ToString());
                tw.WriteLine(mPartRotations[i].M31.ToString() + " " + mPartRotations[i].M32.ToString() + " " + mPartRotations[i].M33.ToString() + " " + mPartRotations[i].M34.ToString());
                tw.WriteLine(mPartRotations[i].M41.ToString() + " " + mPartRotations[i].M42.ToString() + " " + mPartRotations[i].M43.ToString() + " " + mPartRotations[i].M44.ToString());
                tw.WriteLine("");
            }
            tw.Close();
        }

        #endregion

        #region Protected Properties

        protected virtual void OnDeath()
        {
            World.Remove(Sensor);
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

        public Creature(Vector3 position, float height, float radius, float mass, Renderable renderable, RadialSensor radialSensor, Controller controller, int numParts)
            : base(renderable, new Cylinder(position, height, radius, mass))
        {
            Sensor = radialSensor;
            Forward = new Vector3(0.0f, 0.0f, 1.0f);
            mPartAttachments = new List<PartAttachment>(numParts);
            for (int i = 0; i < numParts; ++i)
            {
                mPartAttachments.Add(null);
            }

            mUnusedPartBones = GetUsablePartBones();

            CharacterController = new CharacterController(Entity, 1.0f);
            SlideSlope = CharacterController.SupportFinder.MaximumSlope;

            Controller = controller;
            controller.SetCreature(this);

            for (int i = 0; i < mNumParts; ++i)
            {
                mPartRotations[i] = Matrix.Identity;
                mBoneUp[i] = Vector3.Up;
                mBoneForward[i] = Vector3.Forward;
                mBoneRight[i] = Vector3.Right;
            }
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
                    World.Remove(Sensor);
                    World.Space.Remove(CharacterController);
                }

                base.World = value;

                if (value != null)
                {
                    value.Add(Sensor);
                    value.Space.Add(CharacterController);
                }
            }
        }

        protected virtual Matrix GetRenderTransform()
        {
            return Matrix.CreateScale(Scale) * Entity.WorldTransform;
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
                if (partAttachment != null)
                {
                    int count = 0;
                    foreach (PartBone partBone in partAttachment.Bones)
                    {
                        Matrix worldTransform = (mRenderable as AnimateModel).GetBoneTransform(partBone.ToString()) * GetRenderTransform();
                        partAttachment.Part.SubParts[count].Render(worldTransform);

                        count++;
                    }
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
        /// <param name="slot">The slot in the list to put the part</param>
        public void AddPart(Part part, int slot)
        {
            if (slot >= mPartAttachments.Count())
            {
                return;
            }

            List<PartBone> usedBones = GetPartBonesForPart(part);
            foreach (PartBone partBone in usedBones)
            {
                mUnusedPartBones.Remove(partBone);
            }

            mPartAttachments[slot] = new PartAttachment(part, usedBones);

            part.Creature = this;
        }

        /// <summary>
        /// Remove attached part from the creature.
        /// </summary>
        /// <param name="part"></param>
        public void RemovePart(Part part)
        {
            int slot = -1;
            PartAttachment partAttachment = null;
            for (int i = 0; i < mPartAttachments.Count(); ++i)//PartAttachment attachment in mPartAttachments)
            {
                PartAttachment attachment = mPartAttachments[i];
                if (part == attachment.Part)
                {
                    partAttachment = attachment;
                    slot = i;
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

            mPartAttachments[slot] = null;
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
                Sensor.Forward = Forward;
            }
        }

        /// <summary>
        /// Uses the specified part.
        /// </summary>
        /// <param name="part">The index into the list of parts.</param>
        /// <param name="direction">The direction in which to use the part.</param>
        public virtual void UsePart(int part, Vector3 direction)
        {
            if (part < mPartAttachments.Count() && mPartAttachments[part] != null)
            {
                mPartAttachments[part].Part.Use(direction);
            }
        }

        /// <summary>
        /// Finishes using the specified part.
        /// </summary>
        /// <param name="part">The index into the list of parts.</param>
        /// <param name="direction">The direction in which to use the part.</param>
        public virtual void FinishUsingPart(int part, Vector3 direction)
        {
            if (part < mPartAttachments.Count() && mPartAttachments[part] != null)
            {
                mPartAttachments[part].Part.FinishUse(direction);
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
                }
            }
        }

        /// <summary>
        /// Damages the creature.
        /// </summary>
        /// <param name="damage">The amount of damage dealt.</param>
        public virtual void Damage(int damage)
        {
            if (damage > 0 && !Invulnerable)
            {
                foreach (PartAttachment partAttachment in mPartAttachments)
                {
                    if (partAttachment != null)
                    {
                        partAttachment.Part.Damage(damage);
                    }
                }
                Controller.Damage(damage);
                Invulnerable = true;
                mInvulnerableTimer = InvulnerableLength;
            }
        }

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

            if (mInvulnerableTimer > 0.0f)
            {
                mInvulnerableTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (mInvulnerableTimer < 0.0f)
                {
                    Invulnerable = false;
                }
            }

            float elapsedTime = (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            Sensor.Update(gameTime);
            Controller.Update(gameTime, Sensor.CollidingCreatures);
            Sensor.Position = Position;
            Sensor.Forward = Forward;

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
                if (p != null)
                {
                    p.Part.Update(gameTime);
                }
            }
        }

        #endregion
    }
}
