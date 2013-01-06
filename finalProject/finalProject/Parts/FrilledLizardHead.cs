using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using finalProject.Projectiles;
using GameConstructLibrary;
using Microsoft.Xna.Framework.Audio;

namespace finalProject.Parts
{
    public class FrilledLizardHead : CooldownPart, IRangedPart
    {
        private SoundEffect mSpitSound = SoundManager.LookupSound("spit");

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
                12.0f,
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
            PlayAnimation("spit", false, false);
            mSpitSound.Play();
            mCanAnimate = false;
        }

        public override void TryPlayAnimation(string animationName, bool loop, bool playOnCreature)
        {
            if (animationName == "stand")
            {
                base.TryPlayAnimation("standOpen", loop, playOnCreature);
            }
            else if (animationName != "walk")
            {
                base.TryPlayAnimation(animationName, loop, playOnCreature);
            }
        }

        public override void FinishUse(Vector3 direction)
        {
            mCanAnimate = true;
        }

        public override void Cancel() { }
    }
}
