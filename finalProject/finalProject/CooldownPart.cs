using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphicsLibrary;
using Microsoft.Xna.Framework;

namespace finalProject
{
    public abstract class CooldownPart : Part
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

        public CooldownPart(double cooldownLength, SubPart[] subParts)
            : base(subParts)
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

        protected abstract void UseCooldown(Vector3 direction);
    }
}
