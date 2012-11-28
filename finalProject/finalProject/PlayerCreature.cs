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
        private const float mSneak = 10.0f;

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

        public override float Sneak
        {
            get
            {
                return mSneak;
            }
        }

        public override bool Incapacitated
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Public Methods

        public PlayerCreature(Viewport viewPort, Vector3 position)
            : base(new AnimateModel("playerBean_stand"), new Cylinder(position, 1.8f, 1.2f, 10.0f), new RadialSensor(4.0f), new PlayerController(viewPort))
        {
            (mRenderable as AnimateModel).PlayAnimation("Take 001");
            Scale = new Vector3(0.004f);

            mStandingModel = new AnimateModel("playerBean_stand");
            mStandingModel.PlayAnimation("Take 001");

            mWalkingModel = new AnimateModel("playerBean_walk");
            mWalkingModel.PlayAnimation("Take 001");

            mJumpingModel = new AnimateModel("playerBean_jump");
            mJumpingModel.PlayAnimation("Take 001");
        }

        /// <summary>
        /// Removes parts until no parts are remaining, then kills player.
        /// </summary>
        /// <param name="damage">Amount of damage to apply.</param>
        public override void Damage(int damage)
        {
            while (damage-- > 0)
            {
                if (mParts.Count() == 0)
                {
                    // die?
                    return;
                }

                mParts.Remove(mParts[Rand.rand.Next(mParts.Count())]);
            }
        }
        
        /// <summary>
        /// Adds a part to the PlayerCreature. The part chosen is from the closest incapacitated animal within the radial sensor.
        /// </summary>
        public void FindAndAddPart()
        {
            foreach (Creature cur in mSensor.CollidingCreatures)
            {
                if (cur.Incapacitated)
                {
                    AddPart(cur.Parts[0]);
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

        public override void Render()
        {
            mRenderable.Render(CharacterController.Body.Position + new Vector3(0.0f, -0.2f, 0.0f), -1.0f * XNAOrientationMatrix.Forward, Scale);
        }
        #endregion
    }

    public enum Stance { Standing, Walking, Jumping };
}
