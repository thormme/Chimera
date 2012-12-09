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
    class FrogHead : Part
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
                        Matrix.CreateFromYawPitchRoll(-MathHelper.PiOver2, 0, 0),
                        new Vector3(1.0f)
                    )
                },
                false
            )
        {
            //(mRenderable as AnimateModel).PlayAnimation("Take 001");
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
            PlayAnimation("tongue", true);
            mTongue = new FrogTongue(Creature, direction);
            Creature.World.Add(mTongue);
        }

        

        public override void FinishUse(Vector3 direction)
        {
            if (Creature != null && mTongue != null)
            {
                Creature.World.Remove(mTongue);
            }
            mTongue = null;
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
