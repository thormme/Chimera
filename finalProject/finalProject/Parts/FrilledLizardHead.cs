using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using finalProject.Projectiles;

namespace finalProject.Parts
{
    public class FrilledLizardHead : CooldownPart
    {
        public FrilledLizardHead()
            : base(
                20.0f,
                new Part.SubPart[] {
                    new SubPart(
                        new AnimateModel("lizard_head", "standOpen"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.HeadCenterCap, 
                            Creature.PartBone.HeadLeftCap,
                            Creature.PartBone.HeadRightCap
                        },
                        new Vector3(),
                        Matrix.CreateFromQuaternion(new Quaternion()),
                        new Vector3(4.0f)
                    )
                },
                false,
                new Sprite("frilledLizardIcon")
            )
        { }

        protected override void UseCooldown(Vector3 direction)
        {
            Creature.World.Add(new FrilledLizardVenom(Creature, direction));
            PlayAnimation("spit", true, false);
            mCanAnimate = false;
        }

        public override void FinishUse(Vector3 direction)
        {
            mCanAnimate = true;
        }

        public override void Cancel() { }
    }
}
