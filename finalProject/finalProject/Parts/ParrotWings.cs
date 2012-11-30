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
    class ParrotWings : Part
    {

        private const int flapPower = 50;
        private const int numFlaps = 3;

        private const double flapWait = 0.0f;

        private int mFlaps;
        private double mFlapTimer;

        public ParrotWings()
            : base(
                new Part.SubPart[] {
                    new SubPart(
                        new InanimateModel("sphere"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.ArmLeft1Cap,
                            Creature.PartBone.ArmLeft2Cap,
                            Creature.PartBone.ArmLeft3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromYawPitchRoll(-MathHelper.PiOver2, 0, 0),
                        new Vector3(1.0f, 1.0f, 1.0f)
                    ),
                    new SubPart(
                        new InanimateModel("sphere"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.ArmRight1Cap,
                            Creature.PartBone.ArmRight2Cap,
                            Creature.PartBone.ArmRight3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromYawPitchRoll(-MathHelper.PiOver2, 0, 0),
                        new Vector3(1.0f, 1.0f, 1.0f)
                    )
                }
            )
        {
            //(mRenderable as AnimateModel).PlayAnimation("Take 001");
            mFlaps = 0;
            mFlapTimer = 0.0f;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            mFlapTimer += time.ElapsedGameTime.TotalSeconds;
            if (Creature.CharacterController.SupportFinder.HasSupport) mFlaps = 0;
        }

        public override void Use(Microsoft.Xna.Framework.Vector3 direction)
        {
            if (Creature.CharacterController.SupportFinder.HasSupport) Creature.Jump();
            else if (mFlaps < numFlaps && mFlapTimer > flapWait)
            {
                mFlaps++;
                mFlapTimer = 0.0f;
                Vector3 flap = new Vector3(0.0f, 1.0f * flapPower, 0.0f);
                if (Creature.Entity.LinearVelocity.Y < 0) flap.Y -= Creature.Entity.LinearVelocity.Y;
                Creature.Entity.ApplyLinearImpulse(ref flap);
            }
        }

        public override void FinishUse(Vector3 direction)
        {
            
        }

        protected override void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
