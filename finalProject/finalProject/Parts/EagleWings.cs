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
    class EagleWings : Part
    {

        private const int flapPower = 300;
        private const int numFlaps = 3;

        private const double flapWait = 0.0f;

        private int ResetFrames = 2;
        private int mReset = 0;

        private int mFlaps;
        private double mFlapTimer;
        private bool mGlide;

        public EagleWings()
            : base(
                new Part.SubPart[] {
                    new SubPart(
                        new AnimateModel("eagle_leftWing", "glide"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.ArmLeft1Cap,
                            Creature.PartBone.ArmLeft2Cap,
                            Creature.PartBone.ArmLeft3Cap
                        },
                        new Vector3(0.0f),
                        Matrix.CreateFromYawPitchRoll(0, 0, 0),
                        new Vector3(1.0f)
                    ),
                    new SubPart(
                        new AnimateModel("eagle_rightWing", "glide"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.ArmRight1Cap,
                            Creature.PartBone.ArmRight2Cap,
                            Creature.PartBone.ArmRight3Cap
                        },
                        new Vector3(0.0f),
                        Matrix.CreateFromYawPitchRoll(0, 0, 0),
                        new Vector3(1.0f)
                    )
                },
                false
            )
        {
            mFlaps = 0;
            mFlapTimer = 0.0f;
        }

        public override void Update(GameTime time)
        {
            mFlapTimer += time.ElapsedGameTime.TotalSeconds;

            if (mReset > 0)
            {
                --mReset;
                if (mReset == 0)
                {
                    Creature.CharacterController.JumpSpeed /= 2;
                }
            }

            if (Creature.CharacterController.SupportFinder.HasTraction)
            {
                mFlaps = 0;
                mGlide = false;
            }

            if (mGlide)
            {
                Vector3 direction = new Vector3(0.0f, -Creature.Entity.LinearVelocity.Y, 0.0f);
                Creature.Entity.ApplyLinearImpulse(ref direction);
                PlayAnimation("glide", true);
            }

            base.Update(time);
        }

        public override void Use(Vector3 direction)
        {
            if (mFlaps == 0 && Creature.CharacterController.SupportFinder.HasSupport)
            {
                Creature.CharacterController.JumpSpeed *= 2;
                Creature.Jump();
                mReset = ResetFrames;
                PlayAnimation("flap_air", true);
            }
            else if (mFlaps < numFlaps && mFlapTimer > flapWait)
            {
                mFlaps++;
                mFlapTimer = 0.0f;
                Vector3 flap = new Vector3(0.0f, 1.0f * flapPower, 0.0f);
                if (Creature.Entity.LinearVelocity.Y < 0) flap.Y -= Creature.Entity.LinearVelocity.Y;
                Creature.Entity.ApplyLinearImpulse(ref flap);
                PlayAnimation("flap_air", true);
            }
            mGlide = true;
        }

        public override void FinishUse(Vector3 direction)
        {
            mGlide = false;
        }

        public override void Reset()
        {
            mFlaps = 0;
        }
    }
}
