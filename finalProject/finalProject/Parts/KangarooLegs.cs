using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities.Prefabs;
using GraphicsLibrary;
using GameConstructLibrary;

namespace finalProject.Parts
{

    class KangarooLegs : Part
    {

        public static GameTip mTip = new GameTip(
            new string[] 
                {
                    "Using kangaroo legs while on the ground will begin charging them.",
                    "When you stop using them, you will jump into the air.",
                    "Using them while in the air will cause you to perform a butt slam."
                },
            10.0f);

        protected override GameTip Tip
        {
            get
            {
                return mTip;
            }
        }

        const double maxJumpCharge = 3.0;
        const double jumpStrengthTimerStart = 1.0;
        const double jumpStrength = 3.0;
        const float forwardJumpForce = 30f;
        const float poundForce = 320f;
        const double poundWaitTime = .5;
        const int poundDamage = 1;

        bool mJumpInUse = false;
        bool mPoundInUse = false;
        bool mPoundWaiting = false;
        int mResetJump = -1;
        double mJumpStrengthTimer;
        double mPoundWaitTimer;
        float mJumpMultiplier;

        public double FullJumpTime
        {
            get
            {
                return maxJumpCharge - jumpStrengthTimerStart;
            }
        }

        public KangarooLegs()
            : base(
                new Part.SubPart[] {
                    new SubPart(
                        new AnimateModel("kangaroo_leftLeg", "stand"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.LegFrontLeft3Cap, 
                            Creature.PartBone.LegRearLeft3Cap,
                            Creature.PartBone.LegFrontLeft2Cap, 
                            Creature.PartBone.LegRearLeft2Cap,
                            Creature.PartBone.LegFrontLeft1Cap, 
                            Creature.PartBone.LegRearLeft1Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromQuaternion(new Quaternion()),
                        new Vector3(3.0f)
                    ),
                    new SubPart(
                        new AnimateModel("kangaroo_rightLeg", "stand"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.LegFrontRight3Cap, 
                            Creature.PartBone.LegRearRight3Cap,
                            Creature.PartBone.LegFrontRight2Cap, 
                            Creature.PartBone.LegRearRight2Cap,
                            Creature.PartBone.LegFrontRight1Cap, 
                            Creature.PartBone.LegRearRight1Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromQuaternion(new Quaternion()),
                        new Vector3(3.0f)
                    )
                },
                true,
                new Sprite("KangarooLegsIcon")
            )
        {
        }

        public override void Use(Vector3 direction)
        {
            if (mPoundInUse || mPoundWaiting)
            {
                return;
            }

            if (Creature.CharacterController.SupportFinder.HasSupport)
            {
                mJumpInUse = true;
                mJumpStrengthTimer = jumpStrengthTimerStart;

                PlayAnimation("charge", false, false);

                mCanAnimate = false;
            }
            else
            {
                mPoundWaiting = true;
                mPoundWaitTimer = poundWaitTime;

                PlayAnimation("pound", false, true);

                mCanAnimate = false;
            }
        }

        public override void Update(GameTime time)
        {
            if (mPoundWaiting)
            {
                mPoundWaitTimer -= time.ElapsedGameTime.TotalSeconds;
                Creature.Entity.LinearMomentum = new Vector3();
                if (mPoundWaitTimer <= 0.0)
                {
                    mPoundWaiting = false;
                    mPoundInUse = true;
                    Vector3 poundImpulse = new Vector3(0, -poundForce, 0);
                    Creature.Entity.ApplyLinearImpulse(ref poundImpulse);
                }
            }

            if (mPoundInUse)
            {
                foreach (IGameObject gameObject in Creature.CollidingObjects)
                {
                    Creature otherCreature = gameObject as Creature;
                    if (otherCreature != null)
                    {
                        otherCreature.Damage(poundDamage, Creature);
                    }
                }

                if (Creature.CharacterController.SupportFinder.HasTraction)
                {
                    mPoundInUse = false;
                }
            }

            if (mJumpInUse)
            {
                Creature.Move(new Vector2());
                mJumpStrengthTimer += time.ElapsedGameTime.TotalSeconds;
            }
            mResetJump--;
            if (mResetJump == 0)
            {
                Creature.CharacterController.JumpSpeed /= mJumpMultiplier;
                Creature.CharacterController.JumpForceFactor /= mJumpMultiplier;
            }

            foreach (SubPart subPart in SubParts)
            {
                (subPart.Renderable as AnimateModel).Update(time);
            }

            mCanAnimate = (!mJumpInUse && !mPoundInUse && Creature.CharacterController.SupportFinder.HasSupport && mResetJump < 0);
        }

        public override void FinishUse(Vector3 direction)
        {
            if (mJumpInUse && mResetJump < -5)
            {
                mJumpInUse = false;
                mResetJump = 2;
                mJumpStrengthTimer = mJumpStrengthTimer > maxJumpCharge ? maxJumpCharge : mJumpStrengthTimer;

                mJumpMultiplier = (float)((mJumpStrengthTimer / maxJumpCharge) * jumpStrength);

                Creature.CharacterController.JumpSpeed *= mJumpMultiplier;
                Creature.CharacterController.JumpForceFactor *= mJumpMultiplier;

                Creature.Jump();

                Vector3 pushForward = Creature.Forward * mJumpMultiplier * forwardJumpForce;
                Creature.Entity.ApplyLinearImpulse(ref pushForward);
                
                PlayAnimation("jump", false, false);

                mCanAnimate = false;
            }
        }

        public override void Reset()
        {
            Cancel();
        }

        public override void Cancel()
        {
            mJumpInUse = false;
            mPoundInUse = false;
            mPoundWaiting = false;

            if (mResetJump > 0)
            {
                Creature.CharacterController.JumpSpeed /= mJumpMultiplier;
                Creature.CharacterController.JumpForceFactor /= mJumpMultiplier;
            }

            mResetJump = -1;
        }
    }
}
