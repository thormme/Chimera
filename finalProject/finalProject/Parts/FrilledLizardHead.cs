using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using finalProject.Projectiles;
using GameConstructLibrary;

namespace finalProject.Parts
{
    public class FrilledLizardHead : CooldownPart, IRangedPart
    {

        public static GameTip mTip = new GameTip(
            new string[] 
                {
                    "Using frilled lizard head will spit a projectile.",
                    "Enemies hit by this projectile will not be able",
                    "to use abilities for a short period of time."
                },
            10.0f);

        protected override GameTip Tip
        {
            get
            {
                return mTip;
            }
        }

        public FrilledLizardHead()
            : base(
                20.0f,
                new Part.SubPart[] {
                    new SubPart(
                        new AnimateModel("lizard_head", "standClosed"),
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
                new Sprite("FrilledLizardHeadIcon")
            )
        { }

        protected override void UseCooldown(Vector3 direction)
        {
            Creature.World.Add(new FrilledLizardVenom(Creature, direction));
            PlayAnimation("spit", true, false);
            mCanAnimate = false;
        }

        public override void TryPlayAnimation(string animationName, bool isSaturated, bool playOnCreature)
        {
            if (animationName == "stand")
            {
                base.TryPlayAnimation("standOpen", isSaturated, playOnCreature);
            }
            else if (animationName != "walk")
            {
                base.TryPlayAnimation(animationName, isSaturated, playOnCreature);
            }
        }

        public override void FinishUse(Vector3 direction)
        {
            mCanAnimate = true;
        }

        public override void Cancel() { }
    }
}
