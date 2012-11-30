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
    class FlyingSquirrelBack : Part
    {

        private bool mGliding;

        public FlyingSquirrelBack()
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
            mGliding = false;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {

            if (mGliding && !Creature.CharacterController.SupportFinder.HasSupport)
            {
                //Vector3 direction = new Vector3(0.0f, 100.0f, 0.0f);
                Vector3 direction = new Vector3(0.0f, -Creature.Entity.LinearVelocity.Y, 0.0f);
                Creature.Entity.ApplyLinearImpulse(ref direction);
                Console.WriteLine(direction.ToString());
            }

        }

        public override void Use(Microsoft.Xna.Framework.Vector3 direction)
        {
            mGliding = true;
        }

        public override void FinishUse(Vector3 direction)
        {
            mGliding = false;
        }

        protected override void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
