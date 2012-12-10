#region Using Statements

using System.Collections.Generic;
using System.Linq;
using BEPUphysics.CollisionShapes.ConvexShapes;
using GameConstructLibrary;
using BEPUphysics.Entities.Prefabs;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.EntityStateManagement;
using BEPUphysics.Entities;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using FinalProject;
using System;
using finalProject.Parts;

#endregion

namespace finalProject
{
    /// <summary>
    /// The creature representing the player character.
    /// </summary>
    class PlayerCreature : Creature
    {
        #region Fields

        private const float mPlayerRadius = 1.0f;
        private const int DefaultSneak = 0;
        private const int DefaultIntimidation = 5;
        private const int DamageThreshold = 30;

        private const double StealLength = 3.0f;

        private bool mStealing = false;
        private Creature mStealTarget = null;
        private double mStealTimer = -1.0f;
        private Part mStolenPart = null;
        
        private int mNumHeightModifyingParts = 0;

        private Sprite[] mButtonSprites = new Sprite[3] {
            new Sprite("blueButton"), 
            new Sprite("yellowButton"), 
            new Sprite("redButton") 
        };

        #endregion

        #region Public Properties

        public ChaseCamera Camera
        {
            get
            {
                return (Controller as PlayerController).mCamera;
            }
        }

        public Stance Stance
        {
            get
            {
                return mStance;
            }
            set
            {
                mStance = value;
            }
        }
        private Stance mStance = Stance.Standing;

        public override int Sneak
        {
            get;
            set;
        }

        public override int Intimidation
        {
            get;
            set;
        }

        /// <summary>
        /// The position that the player will respawn when killed.
        /// </summary>
        public Vector3 SpawnOrigin;

        public bool FoundPart
        {
            get
            {
                return mStolenPart != null;
            }
        }

        public bool Stealing
        {
            get { return mStealing; }
        }

        #endregion

        #region Protected Methods

        protected override Matrix GetRenderTransform()
        {
            return Matrix.CreateFromYawPitchRoll(0, 0, 0) * base.GetRenderTransform() * Matrix.CreateTranslation(new Vector3(0.0f, -0.2f, 0.0f));
        }

        //protected override Matrix GetOptionalPartTransforms()
        //{
            //return Matrix.CreateScale(3.0f);
        //}

        protected override List<Creature.PartBone> GetUsablePartBones()
        {
            List<Creature.PartBone> bones = new List<PartBone>();
            bones.Add(PartBone.ArmLeft1Cap);
            bones.Add(PartBone.ArmLeft2Cap);
            bones.Add(PartBone.ArmLeft3Cap);
            bones.Add(PartBone.ArmRight1Cap);
            bones.Add(PartBone.ArmRight2Cap);
            bones.Add(PartBone.ArmRight3Cap);
            bones.Add(PartBone.HeadCenterCap);
            bones.Add(PartBone.HeadLeftCap);
            bones.Add(PartBone.HeadRightCap);
            bones.Add(PartBone.LegFrontLeft1Cap);
            bones.Add(PartBone.LegFrontLeft2Cap);
            bones.Add(PartBone.LegFrontLeft3Cap);
            bones.Add(PartBone.LegFrontRight1Cap);
            bones.Add(PartBone.LegFrontRight2Cap);
            bones.Add(PartBone.LegFrontRight3Cap);
            bones.Add(PartBone.LegRearLeft1Cap);
            bones.Add(PartBone.LegRearLeft2Cap);
            bones.Add(PartBone.LegRearLeft3Cap);
            bones.Add(PartBone.LegRearRight1Cap);
            bones.Add(PartBone.LegRearRight2Cap);
            bones.Add(PartBone.LegRearRight3Cap);
            bones.Add(PartBone.Spine1Cap);
            bones.Add(PartBone.Spine2Cap);
            bones.Add(PartBone.Spine3Cap);

            return bones;
        }

        protected void Die()
        {
            ////Console.WriteLine("Player died.");
            Position = SpawnOrigin;
            mShield = true;
            Poisoned = false;
            CancelParts();
            mSilenced.Reset();
            mImmobilized.Reset();
            Move(Vector2.Zero);
            Entity.LinearMomentum = Vector3.Zero;
            Entity.LinearVelocity = Vector3.Zero;
            mShieldRechargeTimer = -1.0f;
            mPoisonTimer = -1.0f;
            mInvulnerable.Reset();
            Invulnerable = true;
            mInvulnerableTimer = InvulnerableLength;
            Incapacitated = false;
        }

        protected override void CancelParts()
        {
            base.CancelParts();

            DeactivateStealPart();
        }

        #endregion

        #region Public Methods

        public PlayerCreature(Viewport viewPort, Vector3 position, Vector3 facingDirection)
            : base(position, 1.3f, 0.75f, 10.0f, new AnimateModel("playerBean", "stand"), new RadialSensor(4.0f, 135), new PlayerController(viewPort), 3)
        {

            Forward = facingDirection;

            CharacterController.JumpSpeed *= 1.6f;
            CharacterController.JumpForceFactor *= 1.6f;

            Intimidation = DefaultIntimidation;
            Sneak = DefaultSneak;

            SpawnOrigin = position;
        }

        /// <summary>
        /// Constructor for use by the World level loading.
        /// </summary>
        /// <param name="modelName">Name of the model.</param>
        /// <param name="translation">The position.</param>
        /// <param name="orientation">The orientation.</param>
        /// <param name="scale">The amount to scale by.</param>
        /// <param name="extraParameters">Extra parameters.</param>
        public PlayerCreature(String modelName, Vector3 translation, Quaternion orientation, Vector3 scale, string[] extraParameters)
            : this(Game1.Graphics.GraphicsDevice.Viewport, translation, Matrix.CreateFromQuaternion(orientation).Forward)
        {
            Game1.Camera = Camera;
        }

        /// <summary>
        /// Removes parts until no parts are remaining.
        /// </summary>
        /// <param name="damage">Amount of damage to apply.</param>
        public override void Damage(int damage, Creature source)
        {
            bool die = false;
            if (!Invulnerable)
            {
                List<PartAttachment> validParts = new List<PartAttachment>(mPartAttachments.Count());
                foreach (PartAttachment pa in mPartAttachments)
                {
                    if (pa != null)
                    {
                        validParts.Add(pa);
                    }
                }

                int shield = 0;
                if (mShield)
                {
                    shield = 1;
                }

                if (damage > validParts.Count + shield)
                {
                    die = true;
                }
            }
            
            base.Damage(damage, source);

            if (die)
            {
                Die();
            }
        }
        
        /// <summary>
        /// Adds a part to the PlayerCreature. The part chosen is from the closest incapacitated animal within the radial sensor.
        /// </summary>
        public void StealPart(int slot)
        {
            if (FoundPart)
            {
                if (mPartAttachments[slot] != null)
                {
                    RemovePart(mPartAttachments[slot].Part);
                }
                AddPart(mStolenPart, slot);
                mStolenPart = null;
            }
        }

        protected virtual void StealPartsUpdate(GameTime time)
        {
            if (mStealing && !FoundPart)
            {
                if (mStealTarget != null)
                {
                    if (!Sensor.CollidingCreatures.Contains(mStealTarget))
                    {
                        mStealTarget = null;
                    }
                    else
                    {
                        mStealTimer -= time.ElapsedGameTime.TotalSeconds;
                        if (mStealTimer < 0.0f)
                        {
                            PartAttachment pa = mStealTarget.PartAttachments[0];
                            if (pa != null && mStealTarget.PartAttachments.Count > 0)
                            {
                                mStealTarget.RemovePart(pa.Part);
                                mStealTarget = null;
                                mStolenPart = pa.Part;
                                DeactivateStealPart();
                            }
                        }
                    }
                }

                if (mStealTarget == null)
                {
                    if (Sensor.CollidingCreatures.Count > 0)
                    {
                        foreach (Creature creature in Sensor.CollidingCreatures)
                        {
                            if (creature.PartAttachments[0] != null)
                            {
                                mStealTarget = creature;
                                mStealTimer = StealLength;
                                creature.Damage(0, this);
                            }
                        }
                    }
                }
            }
        }

        protected override void RenderParts(Color color, float weight)
        {
            RenderPartsHelper(color, weight, true);
        }

        /// <summary>
        /// Adds part to creature and increases height of body if part modifies height.
        /// </summary>
        public override void  AddPart(Part part, int slot)
        {
 	        base.AddPart(part, slot);

            if (mNumHeightModifyingParts == 0 && part.Height > 0.0f)
            {
                this.CharacterController.Body.Height = mHeight + part.Height;
            }

            mNumHeightModifyingParts += (part.Height > 0.0f) ? 1 : 0;
        }

        /// <summary>
        /// Removes part from creature and decreases height of body if part modifies height.
        /// </summary>
        public override void RemovePart(Part part)
        {
            base.RemovePart(part);

            if (mNumHeightModifyingParts != 0 && part.Height > 0.0f)
            {
                this.CharacterController.Body.Height = mHeight;
            }
            mNumHeightModifyingParts -= (part.Height > 0.0f) ? 1 : 0;
        }

        /// <summary>
        /// Updates physics and animation for next frame.
        /// </summary>
        /// <param name="gameTime">Time elapsed since last frame.</param>
        public override void Update(GameTime gameTime)
        {
            AnimateModel model = mRenderable as AnimateModel;

            //else if (mStance == Stance.Jumping)
            //{
            //    model.PlayAnimation("jump", true);

            //    foreach (PartAttachment part in mPartAttachments)
            //    {
            //        if (part != null)
            //        {
            //            part.Part.TryPlayAnimation("jump", true);
            //        }
            //    }
            //}

            model.Update(gameTime);

            StealPartsUpdate(gameTime);

            base.Update(gameTime);
        }

        public override void TryPlayAnimation(string animationName, bool isSaturated)
        {
            if (animationName == "walk" || animationName == "stand" || animationName == "jump")
            {
                base.TryPlayAnimation(animationName, isSaturated);
            }
        }

        public override void Render()
        {
            base.Render();

            int width = Game1.Graphics.PreferredBackBufferWidth;
            int height = Game1.Graphics.PreferredBackBufferHeight;
            int buttonSize = (int)(0.1f * height);

            Rectangle[] rects = new Rectangle[3] {
                new Rectangle((int)(width - width * 0.3f), (int)(height - height * 0.2f), buttonSize, buttonSize),
                new Rectangle((int)(width - width * 0.2f), (int)(height - height * 0.25f), buttonSize, buttonSize),
                new Rectangle((int)(width - width * 0.1f), (int)(height - height * 0.2f), buttonSize, buttonSize)
            };

            mButtonSprites[0].Render(rects[0]);
            mButtonSprites[1].Render(rects[1]);
            mButtonSprites[2].Render(rects[2]);

            for (int count = 0; count < mPartAttachments.Count() && count < rects.Length; count++)
            {
                if (mPartAttachments[count] != null)
                {
                    mPartAttachments[count].Part.RenderSprite(new Rectangle(
                        (int)(rects[count].Left - rects[count].Width / 2.0f),
                        (int)(rects[count].Top - rects[count].Height / 2.0f),
                        (int)(rects[count].Width * 2.0f),
                        (int)(rects[count].Height * 2.0f))
                    );
                }
            }

        }

        public void ActivateStealPart()
        {
            if (!mStealing && !Silenced && !FoundPart)
            {
                mStealing = true;
            }
        }

        public void DeactivateStealPart()
        {
            if (mStealing)
            {
                mStealing = false;
            }
        }

        #endregion
    }

    public enum Stance { Standing, Walking, Jumping };
}
