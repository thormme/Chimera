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
using BEPUphysics.CollisionRuleManagement;
using Utility;
using BEPUphysics.NarrowPhaseSystems.Pairs;

#endregion

namespace finalProject
{
    public class BoolCounter
    {
        private int mValueCount = 0;

        public bool Value
        {
            get
            {
                return mValueCount > 0;
            }
            set
            {
                if (value)
                {
                    ++mValueCount;
                }
                else if (Value)
                {
                    --mValueCount;
                }
            }
        }

        public void Reset()
        {
            mValueCount = 0;
        }
    }

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

        public delegate void Modification(Creature creature);

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

        #region Fields

        protected const double FlashLength = 0.5f;
        protected const double AlternateFlashLength = FlashLength / 2.0f;
        protected double mFlashTimer = FlashLength;

        protected const double InvulnerableLength = 1.0f;
        protected double mInvulnerableTimer = -1.0f;

        protected const double DefaultStunLength = 3.0f;
        protected double mStunTimer = -1.0f;

        protected const double ShieldRechargeLength = 6.0f;
        protected double mShieldRechargeTimer = -1.0f;
        protected bool mShield = true;

        protected double mPoisonTimer = -1.0f;

        protected Dictionary<Modification, int> mModifications = new Dictionary<Modification, int>();

        protected float mHeight;

        protected List<PartAttachment> mPartAttachments;
        protected List<PartBone> mUnusedPartBones;

        protected GameTip mTip = new GameTip(new string[] {}, 0.0f);
        
        #endregion

        #region Public Properties

        public Controller Controller
        {
            get;
            set;
        }

        public VisionSensor Sensor
        {
            get;
            protected set;
        }

        private Vector3 mForward;
        public override Vector3 Forward
        {
            get
            {
                return mForward;
            }
            set
            {
                if (!Immobilized)
                {
                    if (value != Vector3.Zero)
                    {
                        value.Normalize();
                        mForward = value;
                    }
                    SetSensorForward(mForward);
                }
            }
        }

        public override Matrix XNAOrientationMatrix
        {
            get
            {
                return Utils.GetMatrixFromLookAtVector(Forward);
            }
            set
            {
                Forward = value.Forward;
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

        protected bool mIncapacitated = false;
        public bool Incapacitated
        {
            get
            {
                return mIncapacitated;
            }
            set
            {
                mIncapacitated = value;
                Silenced = value;
                Immobilized = value;
            }
        }

        public abstract int Intimidation
        {
            get;
            set;
        }

        protected BoolCounter mInvulnerable = new BoolCounter();
        public bool Invulnerable
        {
            get
            {
                return mInvulnerable.Value;
            }
            set
            {
                mInvulnerable.Value = value;
            }
        }

        protected BoolCounter mSilenced = new BoolCounter();
        public bool Silenced
        {
            get
            {
                return mSilenced.Value;
            }
            set
            {
                if (value)
                {
                    CancelParts();
                }
                mSilenced.Value = value;
            }
        }

        protected BoolCounter mImmobilized = new BoolCounter();
        public bool Immobilized
        {
            get
            {
                return mImmobilized.Value;
            }
            set
            {
                mImmobilized.Value = value;
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
        
        protected bool mPoisoned;
        public bool Poisoned
        {
            get
            {
                return mPoisoned;
            }
            set
            {
                if (value)
                {
                    Silenced = true;
                    mShield = false;
                    mPoisonTimer = ShieldRechargeLength;
                    mShieldRechargeTimer = ShieldRechargeLength * 2;
                }
                else
                {
                    Silenced = false;
                    mPoisonTimer = -1.0f;
                    mShieldRechargeTimer = ShieldRechargeLength;
                }

                mPoisoned = value;
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
                if (mPartRotations != null)
                {
                    return mPartRotations[mBoneIndex];
                }
                return Matrix.Identity;
            }
            set
            {
                mPartRotations[mBoneIndex] = value;
            }
        }
        private Matrix[] mPartRotations;

        /// <summary>
        /// Up Basis Vector for current part rotation.
        /// </summary>
        public Vector3 BoneUp
        {
            get
            {
                if (mBoneUp != null)
                {
                    return mBoneUp[mBoneIndex];
                }
                return Vector3.Zero;
            }
            set
            {
                mBoneUp[mBoneIndex] = value;
            }
        }
        private Vector3[] mBoneUp;

        /// <summary>
        /// Forward Basis Vector for current part rotation.
        /// </summary>
        public Vector3 BoneForward
        {
            get
            {
                if (mBoneUp != null)
                {
                    return mBoneForward[mBoneIndex];
                }
                return Vector3.Zero;
            }
            set
            {
                mBoneForward[mBoneIndex] = value;
            }
        }
        private Vector3[] mBoneForward;

        /// <summary>
        /// Right Basis Vector for current part rotation.
        /// </summary>
        public Vector3 BoneRight
        {
            get
            {
                if (mBoneRight != null)
                {
                    return mBoneRight[mBoneIndex];
                }
                return Vector3.Zero;
            }
            set
            {
                mBoneRight[mBoneIndex] = value;
            }
        }
        private Vector3[] mBoneRight;

        public int BoneIndex
        {
            get
            {
                return mBoneIndex;
            }
            set
            {
                mBoneIndex = value;
                if (mPartAttachments[0] != null)
                {
                    if (mBoneIndex < 0)
                    {
                        mBoneIndex += mPartAttachments[0].Bones.Count;
                    }
                    mBoneIndex %= mPartAttachments[0].Bones.Count;
                }
            }
        }
        private int mBoneIndex = 0;

        public void WriteBoneTransforms()
        {
            if (mPartAttachments[0] != null)
            {
                TextWriter tw = new StreamWriter("playerBeanPartOrientations.txt");

                tw.WriteLine(mNumParts);
                tw.WriteLine("");
                for (int i = 0; i < mPartAttachments[0].Bones.Count; ++i)
                {
                    tw.WriteLine(mPartAttachments[0].Bones[i].ToString());
                    tw.WriteLine(mPartRotations[i].M11.ToString() + " " + mPartRotations[i].M12.ToString() + " " + mPartRotations[i].M13.ToString() + " " + mPartRotations[i].M14.ToString());
                    tw.WriteLine(mPartRotations[i].M21.ToString() + " " + mPartRotations[i].M22.ToString() + " " + mPartRotations[i].M23.ToString() + " " + mPartRotations[i].M24.ToString());
                    tw.WriteLine(mPartRotations[i].M31.ToString() + " " + mPartRotations[i].M32.ToString() + " " + mPartRotations[i].M33.ToString() + " " + mPartRotations[i].M34.ToString());
                    tw.WriteLine(mPartRotations[i].M41.ToString() + " " + mPartRotations[i].M42.ToString() + " " + mPartRotations[i].M43.ToString() + " " + mPartRotations[i].M44.ToString());
                    tw.WriteLine("");
                }
                tw.Close();
            }
        }

        #endregion

        #region Protected Methods

        protected virtual void SetSensorForward(Vector3 forward)
        {
            Sensor.Forward = forward;
        }

        protected virtual void CancelParts()
        {
            foreach (PartAttachment pa in mPartAttachments)
            {
                if (pa != null)
                {
                    pa.Part.Cancel();
                }
            }
        }

        protected virtual Matrix GetRenderTransform()
        {
            return Matrix.CreateScale(Scale) * XNAOrientationMatrix * Matrix.CreateTranslation(Position);
        }

        protected virtual void RenderParts(Color color, float weight)
        {
            RenderPartsHelper(color, weight, false);
        }

        protected void RenderPartsHelper(Color overlayColor, float overlayColorWeight, bool scale)
        {
            foreach (PartAttachment partAttachment in mPartAttachments)
            {
                if (partAttachment != null)
                {
                    int count = 0;
                    foreach (PartBone partBone in partAttachment.Bones)
                    {
                        Matrix worldTransform = /* mPartRotations[count] * */(mRenderable as AnimateModel).GetBoneTransform(partBone.ToString()) * GetRenderTransform();
                        partAttachment.Part.SubParts[count].Render(worldTransform, overlayColor, overlayColorWeight, scale);

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
        protected List<PartBone> GetPartBonesForPart(Part part)
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

        protected void PlayPartAnimation(string animation, bool isSaturating)
        {
            foreach (PartAttachment pa in mPartAttachments)
            {
                if (pa != null)
                {
                    pa.Part.TryPlayAnimation(animation, isSaturating, false);
                }
            }
        }

        #endregion

        #region Public Methods

        public Creature(Vector3 position, float height, float radius, float mass, Renderable renderable, VisionSensor radialSensor, Controller controller, int numParts)
            : base(renderable, new Cylinder(position, height, radius, mass))
        {
            mHeight = height;

            Sensor = radialSensor;
            CollisionRules.AddRule(Entity, Sensor.Entity, CollisionRule.NoBroadPhase);
            Forward = new Vector3(0.0f, 0.0f, -1.0f);
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

            if (PartAttachments[0] != null)
            {
                for (int i = 0; i < PartAttachments[0].Bones.Count; ++i)
                {
                    mPartRotations[i] = Matrix.Identity;
                    mBoneUp[i] = Vector3.Up;
                    mBoneForward[i] = Vector3.Forward;
                    mBoneRight[i] = Vector3.Right;
                }
            }

            mBoneIndex = 0;
        }

        public override void Render()
        {
            Color color = Color.Black;
            float weight = 0.0f;
            if (Incapacitated)
            {
                weight = 1.0f;
            }
            else if (mFlashTimer > AlternateFlashLength)
            {
                if (Poisoned)
                {
                    color = Color.Violet;
                    weight = (float)(0.5f);// * mPoisonTimer / ShieldRechargeLength);
                }
                else if (!mShield)
                {
                    color = Color.Red;
                    weight = (float)(0.5f);// * mShieldRechargeTimer / ShieldRechargeLength);
                }
            }
            else
            {
                if (Invulnerable)
                {
                    color = Color.White;
                    weight = (float)(0.5f);// * mInvulnerableTimer / InvulnerableLength);
                }
            }

            if (mRenderable != null)
            {
                mRenderable.Render(GetRenderTransform(), color, weight);
            }
            RenderParts(color, weight);
        }

        public virtual void TryPlayAnimation(string animationName, bool isSaturated)
        {
            foreach (PartAttachment part in mPartAttachments)
            {
                if (part != null && !part.Part.CanAnimate)
                {
                    return;
                }
            }
            (mRenderable as AnimateModel).PlayAnimation(animationName, isSaturated);
        }

        public void AddTip()
        {
            if (!mTip.Displayed)
            {
                mTip.Displayed = true;
                Game1.AddTip(mTip);
            }
        }

        /// <summary>
        /// Attach part to the creature.
        /// </summary>
        /// <param name="part">The part to attach.</param>
        /// <param name="slot">The slot in the list to put the part</param>
        public virtual void AddPart(Part part, int slot)
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
            
            //mPartRotations = new Matrix[PartAttachments[0].Bones.Count];
            //mBoneUp = new Vector3[PartAttachments[0].Bones.Count];
            //mBoneForward = new Vector3[PartAttachments[0].Bones.Count];
            //mBoneRight = new Vector3[PartAttachments[0].Bones.Count];
            //for (int i = 0; i < PartAttachments[0].Bones.Count; ++i)
            //{
            //    mPartRotations[i] = Matrix.Identity;
            //    mBoneUp[i] = Vector3.Up;
            //    mBoneForward[i] = Vector3.Forward;
            //    mBoneRight[i] = Vector3.Right;
            //}
             
        }

        public float CollideDistance(Creature creature)
        {
            Vector3 distanceVector = Position - creature.Position;
            distanceVector.Y = 0.0f;
            return distanceVector.Length() - creature.CharacterController.BodyRadius - CharacterController.BodyRadius;
        }

        /// <summary>
        /// Remove attached part from the creature.
        /// </summary>
        /// <param name="part"></param>
        public virtual void RemovePart(Part part)
        {
            int slot = -1;
            PartAttachment partAttachment = null;
            for (int i = 0; i < mPartAttachments.Count(); ++i)
            {
                PartAttachment attachment = mPartAttachments[i];
                if (attachment == null)
                {
                    continue;
                }

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

        /// <summary>
        /// Uses the specified part.
        /// </summary>
        /// <param name="part">The index into the list of parts.</param>
        /// <param name="direction">The direction in which to use the part.</param>
        public virtual void UsePart(int part, Vector3 direction)
        {
            if (part < mPartAttachments.Count() &&
                mPartAttachments[part] != null &&
                !Silenced)
            {
                mPartAttachments[part].Part.Use(direction);
            }
        }

        /// <summary>
        /// Finishes using the specified part.
        /// </summary>
        /// <param name="part">The index into the list of parts.</param>
        /// <param name="direction">The direction in which to use the part.</param>
        public virtual void FinishUsePart(int part, Vector3 direction)
        {
            if (part < mPartAttachments.Count() &&
                mPartAttachments[part] != null)
            {
                mPartAttachments[part].Part.FinishUse(direction);
            }
        }

        public virtual void Jump()
        {
            if (!Immobilized)
            {
                CharacterController.Jump();
                //PlayPartAnimation("jump", true);
            }
        }

        /// <summary>
        /// Moves the creature in a direction corresponding to its facing direction.
        /// </summary>
        /// <param name="direction">The direction to move relative to the facing direction.</param>
        public virtual void Move(Vector2 direction)
        {
            Move(direction, direction);
        }

        /// <summary>
        /// Moves the creature in a direction and makes the creature face a different direction.
        /// </summary>
        public virtual void Move(Vector2 direction, Vector2 facing)
        {
            if (!Immobilized)
            {
                CharacterController.HorizontalMotionConstraint.MovementDirection = direction;
                if (direction != Vector2.Zero)
                {
                    TryPlayAnimation("walk", false);
                    PlayPartAnimation("walk", false);
                }
                else
                {
                    TryPlayAnimation("stand", true);
                    PlayPartAnimation("stand", true);
                }
                Forward = new Vector3(facing.X, 0.0f, facing.Y);
            }
        }

        /// <summary>
        /// Damages the creature.
        /// </summary>
        /// <param name="damage">The amount of damage dealt.</param>
        public virtual void Damage(int damage, Creature source)
        {
            if (Invulnerable)
            {
                damage = 0;
            }

            //Console.WriteLine(this + " took " + damage + " damage from " + source);
            foreach (PartAttachment partAttachment in mPartAttachments)
            {
                if (partAttachment != null)
                {
                    partAttachment.Part.Damage(damage, source);
                }
            }
            Controller.Damage(damage, source);

            if (damage <= 0)
            {
                return;
            }

            if (mShield)
            {
                mShield = false;
                mShieldRechargeTimer = ShieldRechargeLength;
                --damage;
            }

            if (damage > 0)
            {
                Invulnerable = true;
                mInvulnerableTimer = InvulnerableLength;
            }

            List<PartAttachment> validParts = new List<PartAttachment>(mPartAttachments.Count());
            foreach (PartAttachment pa in mPartAttachments)
            {
                if (pa != null)
                {
                    validParts.Add(pa);
                }
            }

            for (; damage > 0 && validParts.Count > 0; --damage)
            {
                PartAttachment pa = validParts[Rand.rand.Next(validParts.Count())];
                RemovePart(pa.Part);
                validParts.Remove(pa);
            }
        }

        public virtual void Stun()
        {
            Move(Vector2.Zero);
            Silenced = true;
            Immobilized = true;
            mStunTimer = DefaultStunLength;
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

            if (mRenderable is AnimateModel)
            {
                (mRenderable as AnimateModel).Update(gameTime);
            }

            mFlashTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (mFlashTimer < 0.0f)
            {
                mFlashTimer = FlashLength;
            }

            if (mInvulnerableTimer > 0.0f)
            {
                mInvulnerableTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (mInvulnerableTimer < 0.0f)
                {
                    Invulnerable = false;
                }
            }

            if (mStunTimer > 0.0f)
            {
                mStunTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (mStunTimer < 0.0f)
                {
                    Silenced = false;
                    Immobilized = false;
                }
            }

            if (mShieldRechargeTimer > 0.0f)
            {
                mShieldRechargeTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (mShieldRechargeTimer < 0.0f)
                {
                    mShield = true;
                }
            }

            if (mPoisonTimer > 0.0f)
            {
                mPoisonTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (mPoisonTimer < 0.0f)
                {
                    Poisoned = false;
                }
            }

            Sensor.Update(gameTime);
            Controller.Update(gameTime, Sensor.CollidingCreatures);
            Sensor.Position = Position;
            //Sensor.Forward = Forward;

            foreach (PartAttachment p in mPartAttachments)
            {
                if (p != null)
                {
                    p.Part.Update(gameTime);
                }
            }
        }

        public override void InitialCollisionDetected(EntityCollidable sender, Collidable other, CollidablePairHandler collisionPair)
        {
            base.InitialCollisionDetected(sender, other, collisionPair);

            foreach (PartAttachment pa in mPartAttachments)
            {
                if (pa != null)
                {
                    pa.Part.InitialCollisionDetected(sender, other, collisionPair);
                }
            }

            //float totalImpulse = 0;
            //foreach (ContactInformation c in collisionPair.Contacts)
            //{
            //    Vector3 normal = -c.Contact.Normal;
            //    normal.Normalize();
            //    normal *= c.NormalImpulse;
            //    totalImpulse += normal.Y;
            //}

            //if (totalImpulse > 300)
            //{
            //    Damage(12, null);
            //}
            //else if (totalImpulse > 200)
            //{
            //    mShield = false;
            //    mShieldRechargeTimer = ShieldRechargeLength;
            //}

            ////Console.WriteLine(totalImpulse);
        }
        
        public void AddModification(Modification add, Modification remove)
        {
            if (!mModifications.ContainsKey(remove))
            {
                mModifications.Add(remove, 0);
            }

            if (mModifications[remove] == 0)
            {
                add(this);
            }

            ++mModifications[remove];
        }

        public void RemoveModification(Modification remove)
        {
            if (!mModifications.ContainsKey(remove))
            {
                throw new ArgumentException("Modification not found.");
            }

            --mModifications[remove];

            if (mModifications[remove] == 0)
            {
                remove(this);
            }
        }

        #endregion
    }
}
