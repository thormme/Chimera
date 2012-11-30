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
        const double maxJumpCharge = 3.0;
        const double jumpStrength = 3.0;
        const float forwardJumpForce = 30f;
        const float poundForce = 320f;
        const double poundWaitTime = .5;
        const int poundDamage = 35;

        bool mJumpInUse = false;
        bool mPoundInUse = false;
        bool mPoundWaiting = false;
        int mResetJump = -1;
        double mJumpStrengthTimer;
        double mPoundWaitTimer;
        float mJumpMultiplier;

        public KangarooLegs()
            : base(
                new Part.SubPart[] {
                    new SubPart(
                        new InanimateModel("kangaroo_leftLeg"),
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
                        new Vector3(1.0f, 1.0f, 1.0f)
                    ),
                    new SubPart(
                        new InanimateModel("kangaroo_rightLeg"),
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
                        new Vector3(1.0f, 1.0f, 1.0f)
                    )
                }
            )
        {
        }

        public override void Use(Vector3 direction)
        {
            if (Creature.CharacterController.SupportFinder.HasSupport)
            {
                mJumpInUse = true;
                mJumpStrengthTimer = 1.0;
            }
            else
            {
                mPoundWaiting = true;
                mPoundWaitTimer = poundWaitTime;
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
                        otherCreature.Damage(poundDamage);
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
        }

        public override void FinishUse(Vector3 direction)
        {
            if (mJumpInUse)
            {
                mJumpInUse = false;
                mResetJump = 2;
                mJumpStrengthTimer = mJumpStrengthTimer > maxJumpCharge ? maxJumpCharge : mJumpStrengthTimer;

                mJumpMultiplier = (float)((mJumpStrengthTimer / maxJumpCharge) * jumpStrength);

                Creature.CharacterController.JumpSpeed *= mJumpMultiplier;
                Creature.CharacterController.JumpForceFactor *= mJumpMultiplier;

                Creature.Jump();

                Vector3 pushForward = direction * mJumpMultiplier * forwardJumpForce;
                Creature.Entity.ApplyLinearImpulse(ref pushForward);
            }
        }

        protected override void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
