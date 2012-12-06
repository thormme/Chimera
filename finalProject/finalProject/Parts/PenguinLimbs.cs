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
        private const float speed = 10.0f;
        private bool mHasTraction;

        public PenguinLimbs()
            : base(
                new Part.SubPart[] {
                    new SubPart(
                        new InanimateModel("sphere"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.Spine1Cap,
                            Creature.PartBone.Spine2Cap,
                            Creature.PartBone.Spine3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromYawPitchRoll(-MathHelper.PiOver2, 0, 0),
                        new Vector3(0.02f)
                    )
                }
            )
        {
            //(mRenderable as AnimateModel).PlayAnimation("Take 001");
            mHasTraction = true;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            
            if (!mHasTraction && Creature.CharacterController.SupportFinder.HasSupport)
            {
                Vector3 direction = new Vector3(0.0f, -speed, 0.0f);
                Creature.Entity.ApplyLinearImpulse(ref direction);
            }

        }

        public override void Use(Microsoft.Xna.Framework.Vector3 direction)
        {
            mHasTraction = false;
            Creature.CharacterController.SupportFinder.MaximumSlope = 0.0f;
        }

        public override void FinishUse(Vector3 direction)
        {
            mHasTraction = true;
            if (Creature != null)
            {
                Creature.CharacterController.SupportFinder.MaximumSlope = Creature.SlideSlope;
            }
        }

        public override void Reset()
        {
            FinishUse(Vector3.Zero);
        }
    }
}
