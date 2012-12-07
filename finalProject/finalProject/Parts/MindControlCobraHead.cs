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
    public class MindControlCobraHead : CooldownPart
    {
        public bool Active
        {
            get
            {
                return projectile != null && projectile.Active;
            }
        }

        private MindControlProjectile projectile;

        public MindControlCobraHead()
            : base(
                MindControlProjectile.ControlLength * 2.0f,
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
                        new Vector3(0.02f)
                    )
                },
                false
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
            Cancel();
        }

        public override void Cancel()
        {
            if (projectile != null)
            {
                projectile.Stop();
            }
        }

        public override void Reset()
        {
            base.Reset();
            Cancel();
        }
    }
}
