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
    class PenguinLimbs : Part
    {

        public static GameTip mTip = new GameTip(
            new string[] 
                {
                    "Using penguin limbs will allow you to slide down hills."
                },
            10.0f);

        protected override GameTip Tip
        {
            get
            {
                return mTip;
            }
        }

        private const float speed = 10.0f;
        private bool mHasTraction;

        public PenguinLimbs()
            : base(
                new Part.SubPart[] {
                    new SubPart(
                        new AnimateModel("penguin_leftFlipper", "stand"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.ArmLeft1Cap,
                            Creature.PartBone.ArmLeft2Cap,
                            Creature.PartBone.ArmLeft3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromYawPitchRoll(0.2094397f, 0.2932154f, 4.272569f),
                        new Vector3(4.0f)
                    ),
                    new SubPart(
                        new AnimateModel("penguin_rightFlipper", "stand"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.ArmRight1Cap,
                            Creature.PartBone.ArmRight2Cap,
                            Creature.PartBone.ArmRight3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromYawPitchRoll(4.188794f, -2.722715f, 4.021243f),
                        new Vector3(4.0f)
                    ),
                    new SubPart(
                        new AnimateModel("penguin_rightFoot", "stand"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.LegRearLeft1Cap,
                            Creature.PartBone.LegRearLeft2Cap,
                            Creature.PartBone.LegRearLeft3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromYawPitchRoll(0, 0, 0),
                        new Vector3(4.0f)
                    ),
                    new SubPart(
                        new AnimateModel("penguin_rightFoot", "stand"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.LegRearRight1Cap,
                            Creature.PartBone.LegRearRight2Cap,
                            Creature.PartBone.LegRearRight3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromYawPitchRoll(0.2094395f, -0.08377583f, 0.04188792f),
                        new Vector3(4.0f)
                    )
                },
                false,
                new Sprite("PenguinLimbsIcon")
            )
        {
            mHasTraction = true;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            
            if (!mHasTraction && Creature.CharacterController.SupportFinder.HasSupport)
            {
                Vector3 direction = new Vector3(0.0f, -speed, 0.0f);
                Creature.Entity.ApplyLinearImpulse(ref direction);
            }
            base.Update(time);
        }

        public override void Use(Microsoft.Xna.Framework.Vector3 direction)
        {
            PlayAnimation("slide", true, true);
            mHasTraction = false;
            Creature.CharacterController.SupportFinder.MaximumSlope = 0.0f;
            mCanAnimate = false;
        }

        public override void FinishUse(Vector3 direction)
        {
            mHasTraction = true;
            if (Creature != null)
            {
                Creature.CharacterController.SupportFinder.MaximumSlope = Creature.SlideSlope;
            }
            mCanAnimate = true;
        }

        public override void Reset()
        {
            Cancel();
        }

        public override void Cancel()
        {
            FinishUse(Vector3.Zero);
        }
    }
}
