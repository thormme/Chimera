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

        private const float mPlayerRadius = 1.0f;
        private const int DefaultSneak = 0;
        private const int DefaultIntimidation = 5;
        private const int DamageThreshold = 30;

        private const double StealLength = 2.5f;

        private bool mStealing = false;
        private Creature mStealTarget = null;
        private double mStealTimer = -1.0f;
        private Part mStolenPart = null;
        
        private int mNumHeightModifyingParts = 0;


        private Part[] mRespawnParts = new Part[3];

        private ConeSensor mPartStealSensor;
        private Matrix mConeOrientation = Matrix.Identity;

        private Sprite mRequirementText = new Sprite("requirements");
        private Sprite mRequirementBoxSprite = new Sprite("requirementBox");
        private Sprite mRequirementSprite = null;
        private Sprite mCheckSprite = new Sprite("check");

        private Sprite[] mButtonSprites = new Sprite[3] {
            new Sprite("blueButton"), 
            new Sprite("yellowButton"), 
            new Sprite("redButton") 
        };

        private ScrollingTransparentModel mSuckModel;

        #region Tips
        GameTip mDamaged = new GameTip(
            new string[] {
                "You have taken damage.",
                "Taking additional damage will cause you to lose a part."
            },
            10.0f);
        GameTip mPlayerDied = new GameTip(
            new string[] {
                "Taking damage when you have no parts will cause you to die."
            },
            10.0f);
        GameTip mCheckpointEncountered = new GameTip(
        new string[] {
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
        GameTip mBearEncountered = new GameTip(
            new string[] {
                "You have encountered a bear.",
                "Be careful, these are beary strong enemies.",
                "Even the strongest players bearly make it away with their lives.",
                "Vanquish this foe and you will gain the ability to bear arms.",
                "You will be able to maul other creatures with your bear hands."
            },
            10.0f);
        GameTip mCheetahEncountered = new GameTip(
            new string[] {
                "You have encountred a cheetah.",
                "Cheetahs are known for their blinding speed."
            },
            10.0f);
        GameTip mCobraEncountered = new GameTip(
            new string[] {
                "You have encountered a cobra.",
                "Cobras are able to charm their foes into a submissive state."
            },
            10.0f);
        GameTip mEagleEncountered = new GameTip(
            new string[] {
                "You have encountered an eagle.",
                "Eagles are able to soar through the sky."
            },
            10.0f);
        GameTip mFrilledLizardEncountered = new GameTip(
            new string[] {
                "You have encountered a frill necked lizard.",
                "Frill necked lizards are able to scare away other creatures.",
                "Their venom also temporarily disables enemies."
            },
            10.0f);
        GameTip mFrogEncountered = new GameTip(
            new string[] {
                "You have encountered a frog.",
                "Frogs are known for their extremely sticky tongues."
            },
            10.0f);
        GameTip mKangarooEncountered = new GameTip(
            new string[] {
                "You have encountered a kangaroo.",
                "Kangaroos are able to jump extremely high.",
                "Be careful, if provoked they will attempt to crush you."
            },
            10.0f);
        GameTip mPenguinEncountered = new GameTip(
            new string[] {
                "You have encountered a penguin.",
                "Penguins are able to slide down hills."
            },
            10.0f);
        GameTip mRhinoEncountered = new GameTip(
            new string[] {
                "You have encountered a rhino.",
                "Rhinos will charge foes inflicting great force on anything in their way."
            },
            10.0f);
        GameTip mTurtleEncountered = new GameTip(
            new string[] {
                "You have encountered a turtle.",
                "Turtles are able to hide in their shell to avoid damage."
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
            : base(position + Vector3.Up * 10.0f, 1.3f, 0.75f, 10.0f, new AnimateModel("playerBean", "stand"), new VisionSensor(4.0f, 135), new PlayerController(viewPort), 3)
        {
            Forward = facingDirection;

            Vector3[] vertices;
            int[] indices;
            CollisionMeshManager.LookupMesh("suckConeCollision", out vertices, out indices);

            MobileMesh coneMesh = new MobileMesh(vertices, indices,
                new AffineTransform(new Vector3(3.0f, 3.0f, 3.0f), Quaternion.Identity, Vector3.Zero), 
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

        public override void  InitialCollisionDetected(EntityCollidable sender, Collidable other, BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler collisionPair)
        {
 	        base.InitialCollisionDetected(sender, other, collisionPair);
            Console.WriteLine(other.Tag);

            if (other.Tag is Checkpoint)
            {
                Game1.AddTip(mCheckpointEncountered);
            }
            if (other.Tag is GoalPoint)
            {
                Game1.AddTip(mGoalPointEncountered);
            }
            if (other.Tag is CharacterSynchronizer)
            {
                if (((other.Tag as CharacterSynchronizer).body.Tag as Bear) != null)
                {
                    Game1.AddTip(mBearEncountered);
                }
                else if (((other.Tag as CharacterSynchronizer).body.Tag as Cheetah) != null)
                {
                    Game1.AddTip(mCheetahEncountered);
                }
                else if (((other.Tag as CharacterSynchronizer).body.Tag as Cobra) != null)
                {
                    Game1.AddTip(mCobraEncountered);
                }
                else if (((other.Tag as CharacterSynchronizer).body.Tag as Eagle) != null)
                {
                    Game1.AddTip(mEagleEncountered);
                }
                else if (((other.Tag as CharacterSynchronizer).body.Tag as FrilledLizard) != null)
                {
                    Game1.AddTip(mFrilledLizardEncountered);
                }
                else if (((other.Tag as CharacterSynchronizer).body.Tag as Frog) != null)
                {
                    Game1.AddTip(mFrogEncountered);
                }
                else if (((other.Tag as CharacterSynchronizer).body.Tag as Kangaroo) != null)
                {
                    Game1.AddTip(mKangarooEncountered);
                }
                else if (((other.Tag as CharacterSynchronizer).body.Tag as Penguin) != null)
                {
                    Game1.AddTip(mPenguinEncountered);
                }
                else if (((other.Tag as CharacterSynchronizer).body.Tag as Rhino) != null)
                {
                    Game1.AddTip(mRhinoEncountered);
                }
                else if (((other.Tag as CharacterSynchronizer).body.Tag as Turtle) != null)
                {
                    Game1.AddTip(mTurtleEncountered);
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
                                mStealTimer = -1.0f;
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
            AnimateModel model = mRenderable as AnimateModel;

            if ((World as GameWorld).Goal != null)
            {
                mRequirementSprite = new Sprite((World as GameWorld).Goal.PartType.Name + "Icon");
            }

            model.Update(gameTime);

            StealPartsUpdate(gameTime);

            float partStealFraction = (float)((StealLength - mStealTimer) / StealLength);
            float rotationIncrease = 0.0f;
            if (mStealTimer >= 0.0f)
            {
                rotationIncrease = partStealFraction * 1.5f;
            }

            mSuckModel.HorizontalVelocity = 1.0f + rotationIncrease;
            mSuckModel.VerticalVelocity = mSuckModel.HorizontalVelocity;

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

            Rectangle[] rects = new Rectangle[3] {
                new Rectangle((int)(width - width * 0.26f), (int)(height - height * 0.2f), buttonSize, buttonSize),
                new Rectangle((int)(width - width * 0.18f), (int)(height - height * 0.25f), buttonSize, buttonSize),
                new Rectangle((int)(width - width * 0.10f), (int)(height - height * 0.2f), buttonSize, buttonSize)
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
