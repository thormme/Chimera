using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities.Prefabs;
using GraphicsLibrary;
using GameConstructLibrary;
using BEPUphysics;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Constraints.TwoEntity.JointLimits;
using finalProject.Projectiles;
using BEPUphysics.BroadPhaseEntries;

namespace finalProject.Parts
{
    class FrogHead : Part, IRangedPart
    {

        private const float tongueLength = 500.0f;
        private FrogTongue mTongue;

        public FrogHead()
            : base(
                new Part.SubPart[] {
                    new SubPart(
                        new AnimateModel("frog_head", "stand"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.HeadCenterCap,
                            Creature.PartBone.HeadLeftCap,
                            Creature.PartBone.HeadRightCap
                        },
                        new Vector3(),
                        Matrix.CreateFromYawPitchRoll(-0.1256637f, 0.2094395f, 7.450581E-09f),
                        new Vector3(5.0f)
                    )
                },
                false,
                new Sprite("FrogHeadIcon")
            )
        {
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            if (mTongue != null)
            {
                if ((mTongue.Entity.Position - Creature.Entity.Position).Length() > 300)
                {
                    FinishUse(Vector3.Zero);
                }
            }
        }

        public override void Use(Vector3 direction)
        {
            PlayAnimation("tongue", false, false);
            mTongue = new FrogTongue(Creature, direction);
            Creature.World.Add(mTongue);
            mCanAnimate = false;
        }

        public override void FinishUse(Vector3 direction)
        {
            if (Creature != null && mTongue != null)
            {
                Creature.World.Remove(mTongue);
            }
            mTongue = null;
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
