using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities.Prefabs;
using GraphicsLibrary;
using GameConstructLibrary;
using BEPUphysics.Entities;
using BEPUphysics.CollisionShapes.ConvexShapes;

namespace finalProject.Parts
{
    class SpittingCobraHead : CooldownPart
    {

        public SpittingCobraHead()
            : base(
                2.0,
                new Part.SubPart[] {
                    new SubPart(
                        new InanimateModel("sphere"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.HeadCenterCap,
                            Creature.PartBone.HeadLeftCap,
                            Creature.PartBone.HeadRightCap,
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

            base.Update(time);
        }

        protected override void UseCooldown(Microsoft.Xna.Framework.Vector3 direction)
        {
            Creature.World.Add(new Projectile(new InanimateModel("box"), new Entity(new BoxShape(0.2f, 0.2f, 0.2f), 1000.0f), Creature, direction, 40000.0f, new Vector3(0.2f, 0.2f, 0.2f)));
        }

        public override void FinishUse(Vector3 direction)
        {

        }
    }
}
