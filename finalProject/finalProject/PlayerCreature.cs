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
        private const int DefaultSneak = 11;
        private const int DefaultIntimidation = 5;
        private const int DamageThreshold = 30;

        private AnimateModel mStandingModel;
        private AnimateModel mWalkingModel;
        private AnimateModel mJumpingModel;

        #endregion

        #region Public Properties

        public ChaseCamera Camera
        {
            get
            {
                return (mController as PlayerController).mCamera;
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

        #endregion

        #region Public Methods

        public PlayerCreature(Viewport viewPort, Vector3 position)
            : base(position, 1.8f, 1.2f, 10.0f, new AnimateModel("playerBean_stand"), new RadialSensor(4.0f), new PlayerController(viewPort))
        {
            (mRenderable as AnimateModel).PlayAnimation("Take 001");
            Scale = new Vector3(0.004f);

            mStandingModel = new AnimateModel("playerBean_stand");
            mStandingModel.PlayAnimation("Take 001");

            mWalkingModel = new AnimateModel("playerBean_walk");
            mWalkingModel.PlayAnimation("Take 001");

            mJumpingModel = new AnimateModel("playerBean_jump");
            mJumpingModel.PlayAnimation("Take 001");

            Intimidation = DefaultIntimidation;
            Sneak = DefaultSneak;
        }

        /// <summary>
        /// Removes parts until no parts are remaining.
        /// </summary>
        /// <param name="damage">Amount of damage to apply.</param>
        public override void Damage(int damage)
        {
            if (Invulnerable)
            {
                return;
            }

            base.Damage(damage);

            while (damage - DamageThreshold > 0)
            {
                if (mPartAttachments.Count() == 0)
                {
                    // die!
                    return;
                }

                mPartAttachments.Remove(mPartAttachments[Rand.rand.Next(mPartAttachments.Count())]);
            }
        }
        
        /// <summary>
        /// Adds a part to the PlayerCreature. The part chosen is from the closest incapacitated animal within the radial sensor.
        /// </summary>
        public void FindAndAddPart()
        {
            foreach (Creature cur in mSensor.CollidingCreatures)
            {
                if (cur.Incapacitated && cur.PartAttachments.Count > 0)
                {
                    cur.RemovePart(cur.PartAttachments[0].Part);
                    AddPart(cur.PartAttachments[0].Part);
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
                mRenderable = mStandingModel;
            }
            else if (mStance == Stance.Walking)
            {
                mRenderable = mWalkingModel;
            }
            else if (mStance == Stance.Jumping)
            {
                mRenderable = mJumpingModel;
            }

            model.Update(gameTime);

            base.Update(gameTime);
        }

        protected override Matrix GetRenderTransform()
        {
            return Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(MathHelper.Pi, 0, 0) * Entity.WorldTransform * Matrix.CreateTranslation(new Vector3(0.0f, -0.2f, 0.0f));
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
