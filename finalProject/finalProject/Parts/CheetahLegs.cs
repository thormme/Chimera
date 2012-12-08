using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphicsLibrary;
using Microsoft.Xna.Framework;

namespace finalProject.Parts
{
    public class CheetahLegs : MeteredPart
    {
        private const float RunGainSpeed = 22.0f;
        private bool mActive = false;

        static void AddCheetahSpeed(Creature creature)
        {
            creature.CharacterController.HorizontalMotionConstraint.Speed += RunGainSpeed;
        }

        static void RemoveCheetahSpeed(Creature creature)
        {
            creature.CharacterController.HorizontalMotionConstraint.Speed -= RunGainSpeed;
        }

        public CheetahLegs()
            : base(
                2.0f,
                4.0f,
                new Part.SubPart[] {
                    new SubPart(
                        new AnimateModel("cheetah_frontLeftLeg", "stand"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.LegFrontLeft1Cap,
                            Creature.PartBone.LegFrontLeft2Cap,
                            Creature.PartBone.LegFrontLeft3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromQuaternion(new Quaternion()),
                        new Vector3(1.0f)
                    ),
                    new SubPart(
                        new AnimateModel("cheetah_frontRightLeg", "stand"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.LegFrontRight1Cap,
                            Creature.PartBone.LegFrontRight2Cap,
                            Creature.PartBone.LegFrontRight3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromQuaternion(new Quaternion()),
                        new Vector3(1.0f)
                    ),
                    new SubPart(
                        new AnimateModel("cheetah_rearLeftLeg", "stand"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.LegRearLeft1Cap,
                            Creature.PartBone.LegRearLeft2Cap,
                            Creature.PartBone.LegRearLeft3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromQuaternion(new Quaternion()),
                        new Vector3(1.0f)
                    ),
                    new SubPart(
                        new AnimateModel("cheetah_rearRightLeg", "stand"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.LegRearRight1Cap,
                            Creature.PartBone.LegRearRight2Cap,
                            Creature.PartBone.LegRearRight3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromQuaternion(new Quaternion()),
                        new Vector3(1.0f)
                    )
                },
                true
            )
        { }
                
        protected override void UseMeter(Vector3 direction)
        {
            if (!mActive)
            {
                Creature.AddModification(AddCheetahSpeed, RemoveCheetahSpeed);
                mActive = true;
                PlayAnimation("run", false);
            }
        }

        protected override void FinishUseMeter()
        {
            if (Creature != null && mActive)
            {
                Creature.RemoveModification(RemoveCheetahSpeed);
                mActive = false;
            }
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
        }

        public override void TryPlayAnimation(string animationName, bool isSaturated)
        {
            if (!mActive && animationName != "jump")
            {
                PlayAnimation(animationName, isSaturated);
            }
        }

        public override void Cancel()
        {
            FinishUseMeter();
        }
    }
}
