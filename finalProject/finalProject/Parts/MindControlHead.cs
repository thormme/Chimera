using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameConstructLibrary;
using GraphicsLibrary;
using BEPUphysics.Entities.Prefabs;
using finalProject.Projectiles;

namespace finalProject.Parts
{
    public class MindControlHead : CooldownPart
    {
        private MindControlProjectile projectile;
        private int prevHealth = 0;

        public MindControlHead()
            : base(
                2.0,
                new Part.SubPart[] {
                    new SubPart(
                        new InanimateModel("sphere"),
                        new Creature.PartBone[]
                        { 
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
        { }

        protected override void UseCooldown(Vector3 direction)
        {
            projectile = new MindControlProjectile(Creature, direction);
            Creature.World.Add(projectile);
        }

        public override void FinishUse(Vector3 direction)
        { }

        public override void Damage(int damage, Creature source)
        {
            base.Damage(damage, source);

            if (projectile != null)
            {
                projectile.Stop();
            }
        }
    }
}
