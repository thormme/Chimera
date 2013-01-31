﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using GameConstructLibrary;

namespace Chimera.Parts
{
    public class TurtleShell : MeteredPart
    {

        public static GameTip mTip = new GameTip(
            new string[] 
                {
                    "Using turtle shell will cause you to become",
                    "invulnerable for a short period of time."
                },
            10.0f);

        public TurtleShell()
            : base(
                3.0f,
                3.0f,
                new Part.SubPart[] {
                    new SubPart(
                        new InanimateModel("turtle_shell"),//"sphere"),
                        new Creature.PartBone[] {
                            Creature.PartBone.Spine1Cap,
                            Creature.PartBone.Spine2Cap,
                            Creature.PartBone.Spine3Cap
                        },
                        new Vector3(-0.163362786f, 0.0188495629f, -0.157079622f),
                        Matrix.CreateFromYawPitchRoll(-0.6702064f, -3.141595f, -1.549852f),
                        new Vector3(4.0f)
                    )
                },
                false,
                new Sprite("TurtleShellIcon")
            )
        { }

        private bool mActive = false;
        protected bool Active
        {
            get
            {
                return mActive;
            }
            set
            {
                if (value)
                {
                    Creature.Move(Vector2.Zero);
                }
                Creature.Silenced = value;
                Creature.Immobilized = value;
                Creature.Invulnerable = value;
                mActive = value;
            }
        }

        protected override void UseMeter(Vector3 direction)
        {
            if (!Active && Creature != null)
            {
                Active = true;
                Creature.TryPlayAnimation("hide", false);
                mCanAnimate = false;
            }
        }

        protected override void FinishUseMeter()
        {
            if (Active)
            {
                Active = false;
                //Console.WriteLine("stopped");
                mCanAnimate = true;
            }
        }

        public override void Reset()
        {
            base.Reset();
            Cancel();
        }

        public override void Cancel()
        {
            FinishUseMeter();
        }

        public override void Damage(int damage, Creature source)
        {
            base.Damage(damage, source);

            if (Active && damage > 0)
            {
                source.Stun();
            }
        }
    }
}