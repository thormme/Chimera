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

namespace finalProject.Parts
{
    class FrogHead : CooldownPart
    {

        private FrogTongue mTongue;

        public FrogHead()
            : base(
                3.0,
                new Part.SubPart[] {
                    new SubPart(
                        new InanimateModel("sphere"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.HeadCenterCap,
                            Creature.PartBone.HeadLeftCap,
                            Creature.PartBone.HeadRightCap
                        },
                        new Vector3(),
                        Matrix.CreateFromYawPitchRoll(-MathHelper.PiOver2, 0, 0),
                        new Vector3(1.0f, 1.0f, 1.0f)
                    )
                }
            )
        {
            //(mRenderable as AnimateModel).PlayAnimation("Take 001");
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);
        }

        protected override void UseCooldown(Microsoft.Xna.Framework.Vector3 direction)
        {
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

        protected override void Reset()
        {
            FinishUse(Vector3.Zero);
        }

    }
}
