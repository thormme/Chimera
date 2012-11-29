using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphicsLibrary;
using Microsoft.Xna.Framework;

namespace finalProject
{
    abstract class CooldownPart : Part
    {
        protected double CooldownLength
        {
            get;
            private set;
        }

        protected double CooldownTimer
        {
            get;
            set;
        }

        protected bool IsReady
        {
            get
            {
                return CooldownTimer < 0.0f;
            }
        }

        public CooldownPart(double cooldownLength, Renderable renderable, Creature.PartBone[] preferredBones, int limbCount, Vector3 position, Matrix orientation, Vector3 scale)
            : base(renderable, preferredBones, limbCount, position, orientation, scale)
        {
            CooldownLength = cooldownLength;
            CooldownTimer = -1.0f;
        }

        public override void Update(GameTime time)
        {
            CooldownTimer -= time.ElapsedGameTime.TotalSeconds;
        }

        public override void Use(Vector3 direction)
        {
            if (IsReady)
            {
                UseCooldown(direction);
                CooldownTimer = CooldownLength;
            }
        }

        public abstract void UseCooldown(Vector3 direction);
    }
}
