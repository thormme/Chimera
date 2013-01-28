using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;

namespace Chimera
{
    /// <summary>
    /// A type of part whos use is limited with a meter and cooldown.
    /// The meter counts down when being used.
    /// After ceasing to use the part for a set cooldown the meter is reset to full.
    /// </summary>
    public abstract class MeteredPart : Part
    {
        private bool mUsing = false;

        protected double MeterLength
        {
            get;
            private set;
        }

        protected double MeterTimer
        {
            get;
            set;
        }

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
        
        /// <summary>
        /// Whether the part is ready to be used.
        /// </summary>
        /// <returns>True if the meter timer has not run out. False if it has.</returns>
        protected bool IsReady()
        {
            return MeterTimer >= 0.0f;
        }

        /// <summary>
        /// Constructs a new MeteredPart.
        /// </summary>
        /// <param name="meterLength">How long the part can be used before recharging.</param>
        /// <param name="cooldownLength">The length of time that must be waited before the meter will reset.</param>
        /// <param name="subParts">The array of SubParts this part contains.</param>
        /// <param name="raisesBody">Whether this part should raise the associated Creature off of the ground.</param>
        /// <param name="partSprite">The GUI sprite used to indicate this Part.</param>
        public MeteredPart(double meterLength, double cooldownLength, Chimera.Part.SubPart[] subParts, bool raisesBody, Sprite partSprite)
            : base(subParts, raisesBody, partSprite)
        {
            MeterLength = meterLength;
            CooldownLength = cooldownLength;
            Reset();
        }

        public override void Use(Vector3 direction)
        {
            if (IsReady() && !mUsing)
            {
                mUsing = true;
                CooldownTimer = CooldownLength;
                UseMeter(direction);
            }
        }

        public override void  FinishUse(Vector3 direction)
        {
            if (mUsing)
            {
                mUsing = false;
                FinishUseMeter();
            }
        }

        public override void Update(GameTime time)
        {
            if (mUsing)
            {
                MeterTimer -= time.ElapsedGameTime.TotalSeconds;
                if (!IsReady())
                {
                    FinishUse(Vector3.Zero);
                }
            }
            else
            {
                CooldownTimer -= time.ElapsedGameTime.TotalSeconds;
                if (CooldownTimer <= 0.0f)
                {
                    MeterTimer = MeterLength;
                }
            }

            base.Update(time);
        }

        public override void RenderSprite(Rectangle bounds)
        {
            Color color;
            float percentage;
            if (CooldownTimer >= 0.2f)
            {
                color = Color.Black;
                percentage = 1.0f - (float)(MeterTimer / MeterLength);
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

        /// <summary>
        /// Called when the part is used and the meter has not run out.
        /// </summary>
        /// <param name="direction">The direction in which to use the part.</param>
        protected abstract void UseMeter(Vector3 direction);

        /// <summary>
        /// Called when the part is done being used, either when the player released the button
        /// or the meter ran out.
        /// CREATURE IS ALLOWED TO BE NULL when this function is called; it is called in Reset()
        /// which is called in the constructor.
        /// </summary>
        protected abstract void FinishUseMeter();

        public override void Reset()
        {
            MeterTimer = MeterLength;
            CooldownTimer = -1.0f;
            FinishUseMeter();
        }
    }
}
