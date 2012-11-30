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
    class RhinoHead : Part
    {
        private const double CoolDownTime = 2.0f;
        private double mCoolDownTimer;

        public RhinoHead()
            : base(
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
            mCoolDownTimer = -1.0;
            //(mRenderable as AnimateModel).PlayAnimation("Take 001");
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            //(mRenderable as AnimateModel).Update(time);
            mCoolDownTimer -= time.ElapsedGameTime.TotalSeconds;

            if (mCoolDownTimer > 0.0f)
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
        }

        public override void Use(Microsoft.Xna.Framework.Vector3 direction)
        {
            if (mCoolDownTimer <= 0.0f)
            {
                Vector3 impulse = Creature.Forward * 3f;
                Creature.Entity.ApplyLinearImpulse(ref impulse);
            }
        }
    }
}
