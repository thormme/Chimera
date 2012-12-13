﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameConstructLibrary;
using GraphicsLibrary;
using BEPUphysics.Entities.Prefabs;
using finalProject.Projectiles;
using Microsoft.Xna.Framework.Audio;

namespace finalProject.Parts
{
    public class CobraHead : CooldownPart, IRangedPart
    {
        private SoundEffect mSpitSound = SoundManager.LookupSound("spit");

        public static GameTip mTip = new GameTip(
            new string[] 
                {
                    "Using cobra head will allow you to charm an enemy.",
                    "They will be under your complete control for the duration."
                },
            10.0f);

        protected override GameTip Tip
        {
            get
            {
                return mTip;
            }
        }

        public bool Active
        {
            get
            {
                return projectile != null && projectile.Active;
            }
        }

        private MindControlProjectile projectile;

        public CobraHead()
            : base(
                MindControlProjectile.ControlLength * 2.0f,
                new Part.SubPart[] {
                    new SubPart(
                        new AnimateModel("cobra_head", "stand"),
                        new Creature.PartBone[]
                        { 
                            Creature.PartBone.HeadCenterCap,
                            Creature.PartBone.HeadLeftCap,
                            Creature.PartBone.HeadRightCap,
                        },
                        new Vector3(),
                        Matrix.CreateFromYawPitchRoll(0, 0, 0),
                        new Vector3(6.0f)
                    )
                },
                false,
                new Sprite("CobraHeadIcon")
            )
        { }

        protected override void UseCooldown(Vector3 direction)
        {
            PlayAnimation("spit", true, false);
            mSpitSound.Play();
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

        public override void Update(GameTime time)
        {
            base.Update(time);
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
