using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphicsLibrary;
using Microsoft.Xna.Framework;

namespace Chimera
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

        public CooldownPart(double cooldownLength, SubPart[] subParts, bool raisesBody, Sprite partSprite)
            : base(subParts, raisesBody, partSprite)
        {
            CooldownLength = cooldownLength;
            Reset();
        }

        public override void Update(GameTime time)
        {
            CooldownTimer -= time.ElapsedGameTime.TotalSeconds;
            base.Update(time);
        }

        public override void RenderSprite(Rectangle bounds)
        {
            Color color;
            float percentage;
            if (CooldownTimer >= 0.2f)
            {
                color = Color.Black;
                percentage = 0.5f + 0.5f * (float)(CooldownTimer / CooldownLength);
            }
            else if (CooldownTimer > 0.1f)
            {
                color = Color.White;
                percentage = 1.0f - (float)(CooldownTimer) * 5.0f;
            }
            else if (CooldownTimer > 0.0f)
            {
                color = Color.White;
                percentage = (float)(CooldownTimer) * 10.0f;
            }
            else
            {
                color = Color.Black;
                percentage = 0.0f;
            }
            mSprite.Render(bounds, color, percentage);
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

        public override void Reset()
        {
            CooldownTimer = -1.0f;
        }
    }
}
