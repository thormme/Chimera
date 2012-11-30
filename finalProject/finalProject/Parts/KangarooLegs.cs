using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities.Prefabs;
using GraphicsLibrary;

namespace finalProject.Parts
{
    class KangarooLegs : Part
    {
        bool mInUse;
        int mResetJump;
        double mJumpStrengthTimer;
        float mDefaultJumpSpeed;
        float mDefaultJumpForceFactor;

        const double maxJumpCharge = 3.0;
        const double jumpStrength = 3.0;
        const float forwardJumpForce = 30f;
        const float poundVelocity = 2f;

        public KangarooLegs()
            : base(
                new Part.SubPart[] {
                    new SubPart(
                        new InanimateModel("sphere"),
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
                        new Vector3(0.25f, 0.25f, 0.25f)
                    ),
                    new SubPart(
                        new InanimateModel("sphere"),
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
                        new Vector3(0.25f, 0.25f, 0.25f)
                    )
                }
            )
        {
        }

        public override void Use(Vector3 direction)
        {
            if (Creature.CharacterController.SupportFinder.HasSupport)
            {
                mInUse = true;
                mJumpStrengthTimer = 1.0;
            }
            else
            {
                Creature.Entity.LinearMomentum = new Vector3(0, -poundVelocity, 0);
            }
        }

        public override void Update(GameTime time)
        {
            if (mInUse)
            {
                Creature.Move(new Vector2());
                mJumpStrengthTimer += time.ElapsedGameTime.TotalSeconds;
            }
            mResetJump--;
            if (mResetJump == 0)
            {
                Creature.CharacterController.JumpSpeed = mDefaultJumpSpeed;
                Creature.CharacterController.JumpForceFactor = mDefaultJumpForceFactor;
            }
        }

        public override void FinishUse(Vector3 direction)
        {
            mInUse = false;
            mResetJump = 2;
            mJumpStrengthTimer = mJumpStrengthTimer > maxJumpCharge ? maxJumpCharge : mJumpStrengthTimer;

            float multiplier = (float)((mJumpStrengthTimer / maxJumpCharge) * jumpStrength);

            mDefaultJumpSpeed = Creature.CharacterController.JumpSpeed;
            mDefaultJumpForceFactor = Creature.CharacterController.JumpForceFactor;

            Creature.CharacterController.JumpSpeed *= multiplier;
            Creature.CharacterController.JumpForceFactor *= multiplier;

            Creature.Jump();

            Vector3 pushForward = direction * multiplier * forwardJumpForce;
            Creature.Entity.ApplyLinearImpulse(ref pushForward);
        }
    }
}
