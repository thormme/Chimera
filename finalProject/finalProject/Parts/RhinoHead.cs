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

        public RhinoHead()
            : base(
                2.0,
                new Part.SubPart[] {
                    new SubPart(
                        new InanimateModel("sphere"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.ArmLeft1Cap,
                            Creature.PartBone.ArmLeft2Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromYawPitchRoll(-MathHelper.PiOver2, 0, 0),
                        new Vector3(0.5f, 0.5f, 0.5f)
                    ),
                    new SubPart(
                        new InanimateModel("box"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.ArmLeft1Cap,
                            Creature.PartBone.ArmLeft2Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromYawPitchRoll(-MathHelper.PiOver2, 0, 0),
                        new Vector3(20.5f, 20.5f, 20.5f)
                    )
                }
            )
        {
            //(mRenderable as AnimateModel).PlayAnimation("Take 001");
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            //(mRenderable as AnimateModel).Update(time);

            if (CooldownTimer > 0.0f)
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
