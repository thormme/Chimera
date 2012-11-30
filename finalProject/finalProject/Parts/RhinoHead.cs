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
    class RhinoHead : CooldownPart
    {
        
        private const float damageStartTime = 0.2f;
        private const float damageEndTime = 0.6f;

        private float mAttackTimer;

        public RhinoHead()
            : base(
                2.0,
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
            //(mRenderable as AnimateModel).Update(time);

            mAttackTimer += (float)time.ElapsedGameTime.TotalSeconds;

            if (mAttackTimer >= damageStartTime && mAttackTimer <= damageEndTime)
            {
                foreach (IGameObject gameObject in Creature.CollidingObjects)
                {
                    Creature otherCreature = gameObject as Creature;
                    if (otherCreature != null)
                    {
                        Vector3 velocityDifference = Creature.Entity.LinearVelocity - otherCreature.Entity.LinearVelocity;
                        otherCreature.Damage((int)velocityDifference.Length()/*TODO: scale some*/);
                    }
                }
            }

            base.Update(time);
        }

        protected override void UseCooldown(Microsoft.Xna.Framework.Vector3 direction)
        {
            Vector3 impulse = Creature.Forward * 300f;
            Creature.Entity.ApplyLinearImpulse(ref impulse);
        }

        public override void FinishUse(Vector3 direction)
        {
        }
    }
}
