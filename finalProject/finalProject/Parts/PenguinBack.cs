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
    class PenguinBack : Part
    {

        private bool mHasTraction;

        public PenguinBack()
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
                        new Vector3(1.0f, 1.0f, 1.0f)
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
                Vector3 direction = new Vector3(0.0f, -100.0f, 0.0f);
                Creature.Entity.ApplyLinearImpulse(ref direction);
            }

        }

        public override void Use(Microsoft.Xna.Framework.Vector3 direction)
        {
            mHasTraction = false;
        }

        public override void FinishUse(Vector3 direction)
        {
            mHasTraction = true;
        }

        protected override void Reset()
        {
            mHasTraction = true;
        }
    }
}
