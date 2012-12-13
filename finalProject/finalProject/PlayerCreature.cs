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
using finalProject.Creatures;
using BEPUphysics.MathExtensions;
using BEPUphysics.CollisionRuleManagement;

#endregion

namespace finalProject
{
    /// <summary>
    /// The creature representing the player character.
    /// </summary>
    public class PlayerCreature : Creature
    {
        #region Fields

        private bool mHackStop = true;

        private const int NumParts = 3;

        private const float mPlayerRadius = 1.0f;
        private const int DefaultSneak = 0;
        private const int DefaultIntimidation = 5;
        private const int DamageThreshold = 30;

        private const double StealLength = 2.5f;
        private const double LoseLength = 0.2f;

        private bool mStealing = false;
        private Creature mStealTarget = null;
        private double mStealTimer = -1.0f;
        private Part mStolenPart = null;
        private double mLoseTargetTimer = -1.0f;
        
        private int mNumHeightModifyingParts = 0;


        private Part[] mRespawnParts = new Part[NumParts];

        private ConeSensor mPartStealSensor;
        private Matrix mConeOrientation = Matrix.Identity;

        private Sprite mRequirementText = new Sprite("requirements");
        private Sprite mRequirementBoxSprite = new Sprite("requirementBox");
        private Sprite mRequirementSprite = null;
        private Sprite mCheckSprite = new Sprite("check");

        private Sprite[] mButtonSprites = new Sprite[NumParts] {
            new Sprite("blueButton"), 
            new Sprite("yellowButton"), 
            new Sprite("redButton") 
        };

        private ScrollingTransparentModel mSuckModel;

        #region Tips
        GameTip mDamaged = new GameTip(
            new string[] 
            {
                "You have taken damage.",
                "Taking additional damage will cause you to lose a part.",
                "Taking damage with no parts will cause you to die."
            },
            10.0f);
        GameTip mPlayerDied = new GameTip(
            new string[] 
{
                "You have died.",
                "When you die, you return to a checkpoint."
            },
            10.0f);
        GameTip mCheckpointEncountered = new GameTip(
        new string[] 
{
                "You have encountered a checkpoint.",
                "You will respawn at the last checkpoint you touched."
            },
            10.0f);
        GameTip mGoalPointEncountered = new GameTip(
            new string[] {
                "You have found an extraction point.",
                "Return here once you have the required part."
            },
            10.0f);
        #endregion

        #endregion

        #region Public Properties

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
                    World.Remove(mPartStealSensor);
                }

                if (value != null)
                {
                    value.Add(mPartStealSensor);
                }

                base.World = value;
            }
        }

        public ChaseCamera Camera
        {
            get
            {
                if (Controller is PlayerController)
                {
                    return (Controller as PlayerController).mCamera;
                }
                else
                {
                    return null;
                }
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
        private Vector3 mSpawnOrigin;
        public Vector3 SpawnOrigin
        {
            get
            {
                return mSpawnOrigin;
            }
            set
            {
                mSpawnOrigin = value;
                for (int slot = 0; slot < NumParts; ++slot)
                {
                    if (mPartAttachments[slot] != null)
                    {
                        mRespawnParts[slot] = mPartAttachments[slot].Part;
                    }
                    else
                    {
                        mRespawnParts[slot] = null;
                    }
                }
            }
        }

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

        protected override void CancelParts()
        {
            base.CancelParts();

            DeactivateStealPart();
        }

        protected void ResetStealTarget()
        {
            if (mStealTarget != null)
            {
                mStealTarget = null;
                Console.WriteLine("lost target");
            }
            mStealTimer = -1.0f;
        }

        public override void AddTip()
        {
            
        }

        #endregion

        #region Public Methods

        public void Die()
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

            for (int slot = 0; slot < NumParts; ++slot)
            {
                if (mPartAttachments[slot] != null)
                {
                    RemovePart(mPartAttachments[slot].Part);
                }
            }

            for (int slot = 0; slot < NumParts; ++slot)
            {
                if (mRespawnParts[slot] != null)
                {
                    AddPart(mRespawnParts[slot], slot);
                }
            }
        }

        public PlayerCreature(Viewport viewPort, Vector3 position, Vector3 facingDirection)
            : base(position + Vector3.Up * 10.0f, 1.3f, 0.75f, 10.0f, new AnimateModel("playerBean", "stand"), new VisionSensor(4.0f, 135), new PlayerController(viewPort), NumParts)
        {
            InputAction.Enabled = false;
            Forward = facingDirection;

            Vector3[] vertices;
            int[] indices;
            CollisionMeshManager.LookupMesh("suckConeCollision", out vertices, out indices);

            MobileMesh coneMesh = new MobileMesh(vertices, indices,
                new AffineTransform(new Vector3(3.0f), Quaternion.Identity, Vector3.Zero), 
                BEPUphysics.CollisionShapes.MobileMeshSolidity.Clockwise);

            mPartStealSensor = new ConeSensor(coneMesh);
            CollisionRules.AddRule(Entity, mPartStealSensor.Entity, CollisionRule.NoBroadPhase);

            CharacterController.JumpSpeed *= 1.6f;
            CharacterController.JumpForceFactor *= 1.6f;

            Intimidation = DefaultIntimidation;
            Sneak = DefaultSneak;

            SpawnOrigin = position;

            mSuckModel = new ScrollingTransparentModel("suckCone");
            mSuckModel.HorizontalVelocity = 1.0f;
            mSuckModel.VerticalVelocity = 1.0f;
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

        public override void InitialCollisionDetected(EntityCollidable sender, Collidable other, BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler collisionPair)
        {
            if (mHackStop)
            {
                mHackStop = false;
                InputAction.Enabled = true;
            }

 	        base.InitialCollisionDetected(sender, other, collisionPair);
            //Console.WriteLine(other.Tag);

            if (other.Tag is Checkpoint)
            {
                Game1.AddTip(mCheckpointEncountered);
            }
            else if (other.Tag is GoalPoint)
            {
                Game1.AddTip(mGoalPointEncountered);
            }
            else if (other.Tag is CharacterSynchronizer)
            {
                if ((other.Tag as CharacterSynchronizer).body.Tag is Creature)
                {
                    ((other.Tag as CharacterSynchronizer).body.Tag as Creature).AddTip();
                }
            }
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
                if (!mPlayerDied.Displayed)
                {
                    Game1.AddTip(mPlayerDied);
                }
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
                    if (!mPartStealSensor.CollidingCreatures.Contains(mStealTarget))
                    {
                        if (mLoseTargetTimer > 0.0f)
                        {
                            mLoseTargetTimer -= time.ElapsedGameTime.TotalSeconds;
                            if (mLoseTargetTimer < 0.0f)
                            {
                                ResetStealTarget();
                            }
                        }
                        else
                        {
                            mLoseTargetTimer = LoseLength;
                        }
                    }
                    else
                    {
                        mLoseTargetTimer = -1.0f;
                        mStealTimer -= time.ElapsedGameTime.TotalSeconds;
                        if (mStealTimer < 0.0f)
                        {
                            PartAttachment pa = mStealTarget.PartAttachments[0];
                            if (pa != null && mStealTarget.PartAttachments.Count > 0)
                            {
                                mStealTarget.RemovePart(pa.Part);
                                ResetStealTarget();
                                mStolenPart = pa.Part;
                                DeactivateStealPart();
                            }
                        }
                    }
                }

                if (mStealTarget == null)
                {
                    mStealTimer = -1.0f;
                    if (mPartStealSensor.CollidingCreatures.Count > 0)
                    {
                        foreach (Creature creature in mPartStealSensor.CollidingCreatures)
                        {
                            if (creature.PartAttachments[0] != null)
                            {
                                mStealTarget = creature;
                                mStealTimer = StealLength;
                                creature.Damage(0, this);
                                //Console.WriteLine("found target");
                                break;
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
        public override void AddPart(Part part, int slot)
        {
 	        base.AddPart(part, slot);

            part.AddTip();

            if (mNumHeightModifyingParts == 0 && part.Height > 0.0f)
            {
                CharacterController.Body.Height = mHeight + part.Height;
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
                CharacterController.Body.Height = mHeight;
            }
            mNumHeightModifyingParts -= (part.Height > 0.0f) ? 1 : 0;
        }

        /// <summary>
        /// Updates physics and animation for next frame.
        /// </summary>
        /// <param name="gameTime">Time elapsed since last frame.</param>
        public override void Update(GameTime gameTime)
        {
            //if (mHackStop)
            //{
            //    return;
            //}

            if ((World as GameWorld).Goal != null)
            {
                mRequirementSprite = new Sprite((World as GameWorld).Goal.PartType.Name + "Icon");
            }

            StealPartsUpdate(gameTime);

            float partStealFraction = (float)((StealLength - mStealTimer) / StealLength);
            float rotationIncrease = 0.0f;
            if (mStealTimer >= 0.0f)
            {
                rotationIncrease = partStealFraction * 1.5f;
            }

            mSuckModel.VerticalVelocity = 1.0f + rotationIncrease;
            mSuckModel.HorizontalVelocity = mSuckModel.VerticalVelocity;

            if (Controller is PlayerController)
            {
                mConeOrientation = Matrix.CreateScale(new Vector3(3.0f, 3.0f, 3.0f));
                mConeOrientation *= Matrix.CreateRotationX(MathHelper.PiOver2);
                mConeOrientation *= Matrix.CreateWorld(Position + Camera.Forward * 6.0f, Camera.Forward, Vector3.Cross(Camera.Right, Camera.Forward));

                mPartStealSensor.Update(gameTime);

                mPartStealSensor.Position = Position + Camera.Forward * 6.0f;
                mPartStealSensor.XNAOrientationMatrix = Matrix.CreateRotationX(MathHelper.Pi);
                mPartStealSensor.XNAOrientationMatrix *= Matrix.CreateWorld(Position + Camera.Forward * 6.0f, Camera.Forward, Vector3.Cross(Camera.Right, Camera.Forward));
            }

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
            if (mStealing)
            {
                mSuckModel.Render(mConeOrientation);
            }

            base.Render();
            RenderRequirement();
            RenderAbilities();
        }

        private void RenderRequirement()
        {
            int width = Game1.Graphics.PreferredBackBufferWidth;
            int height = Game1.Graphics.PreferredBackBufferHeight;
            Rectangle textRect = new Rectangle((int)(height * 0.04f), (int)(height * 0.02f), (int)(128 * width / 1280), (int)(26 * height / 720));
            Rectangle boxRect = new Rectangle((int)(height * 0.046f), (int)(height * 0.04f), (int)(0.16f * height), (int)(0.16f * height));

            mRequirementText.Render(textRect);
            mRequirementBoxSprite.Render(boxRect);
            if (mRequirementSprite != null)
            {
                mRequirementSprite.Render(boxRect);
            }

            bool retrieved = false;
            foreach (PartAttachment part in mPartAttachments)
            {
                if (part != null)
                {
                    if (part.Part.GetType() == (World as GameWorld).Goal.PartType)
                    {
                        retrieved = true;
                    }
                }
            }

            if (retrieved)
            {
                mCheckSprite.Render(boxRect);
            }

        }

        private void RenderAbilities()
        {

            int width = Game1.Graphics.PreferredBackBufferWidth;
            int height = Game1.Graphics.PreferredBackBufferHeight;
            int buttonSize = (int)(0.1f * height);

            if (FoundPart)
            {
                Sprite black = new Sprite("black");
                black.Render(new Rectangle((int)(width - height * 0.55f), (int)(height - height * 0.47f), (int)(0.50f * height), (int)(0.42f * height)));
                Sprite white = new Sprite("white");
                white.Render(new Rectangle((int)(width - height * 0.54f), (int)(height - height * 0.46f), (int)(0.48f * height), (int)(0.40f * height)));
                Sprite assign = new Sprite("assign");
                assign.Render(new Rectangle((int)(width - height * 0.52), (int)(height - height * 0.42f), (int)(0.44f * height), (int)(0.1f * height)));
            }

            Rectangle[] rects = new Rectangle[3] {
                new Rectangle((int)(width - height * 0.5f), (int)(height - height * 0.2f), buttonSize, buttonSize),
                new Rectangle((int)(width - height * 0.35f), (int)(height - height * 0.25f), buttonSize, buttonSize),
                new Rectangle((int)(width - height * 0.2f), (int)(height - height * 0.2f), buttonSize, buttonSize)
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
                ResetStealTarget();
            }
        }

        #endregion
    }

    public enum Stance { Standing, Walking, Jumping };
}
