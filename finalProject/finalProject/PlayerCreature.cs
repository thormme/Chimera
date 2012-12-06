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

        public override bool Incapacitated
        {
            get
            {
                return false;
            }
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

        #endregion

        #region Public Methods

        public PlayerCreature(Viewport viewPort, Vector3 position, Vector3 facingDirection)
            : base(position, 1.3f, 0.75f, 10.0f, new AnimateModel("playerBean", "stand"), new RadialSensor(4.0f, 135), new PlayerController(viewPort), 3)
        {
            Scale = new Vector3(1.0f);
            Forward = facingDirection;

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
            if (Invulnerable)
            {
                return;
            }

            base.Damage(damage, source);

            if (damage - DamageThreshold > 0)
            {
                List<PartAttachment> validParts = new List<PartAttachment>(mPartAttachments.Count());
                foreach (PartAttachment pa in mPartAttachments)
                {
                    if (pa != null)
                    {
                        validParts.Add(pa);
                    }
                }

                if (validParts.Count() == 0)
                {
                    // die!
                    Position = SpawnOrigin;
                    return;
                }

                //RemovePart(validParts[Rand.rand.Next(validParts.Count())].Part);
            }
        }
        
        /// <summary>
        /// Adds a part to the PlayerCreature. The part chosen is from the closest incapacitated animal within the radial sensor.
        /// </summary>
        public void FindAndAddPart(int slot)
        {
            foreach (Creature creature in Sensor.CollidingCreatures)
            {
                PartAttachment pa = creature.PartAttachments[0];
                if (pa != null && creature.PartAttachments.Count > 0)
                {
                    creature.RemovePart(pa.Part);
                    if (mPartAttachments[slot] != null)
                    {
                        RemovePart(mPartAttachments[slot].Part);
                    }
                    AddPart(pa.Part, slot);
                    return;
                }
            }
        }

        /// <summary>
        /// Updates physics and animation for next frame.
        /// </summary>
        /// <param name="gameTime">Time elapsed since last frame.</param>
        public override void Update(GameTime gameTime)
        {
            AnimateModel model = mRenderable as AnimateModel;

            if (mStance == Stance.Standing)
            {
                model.PlayAnimation("stand", true);

                foreach (PartAttachment part in mPartAttachments)
                {
                    if (part != null)
                    {
                        part.Part.TryPlayAnimation("stand", true);
                    }
                }
            }
            else if (mStance == Stance.Walking)
            {
                model.PlayAnimation("walk", false);

                foreach (PartAttachment part in mPartAttachments)
                {
                    if (part != null)
                    {
                        part.Part.TryPlayAnimation("walk", false);
                    }
                }
            }
            else if (mStance == Stance.Jumping)
            {
                model.PlayAnimation("jump", true);

                foreach (PartAttachment part in mPartAttachments)
                {
                    if (part != null)
                    {
                        part.Part.TryPlayAnimation("jump", true);
                    }
                }
            }

            model.Update(gameTime);

            base.Update(gameTime);
        }

        protected override Matrix GetRenderTransform()
        {
            return Matrix.CreateFromYawPitchRoll(0, 0, 0) * base.GetRenderTransform() * Matrix.CreateTranslation(new Vector3(0.0f, -0.2f, 0.0f));
        }

        protected override Matrix GetOptionalTransforms()
        {
            return Matrix.CreateScale(4.0f) * Matrix.CreateRotationY(MathHelper.Pi);
        }

        #endregion

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
    }

    public enum Stance { Standing, Walking, Jumping };
}
