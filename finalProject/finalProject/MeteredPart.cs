using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace finalProject
{
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

        protected bool IsReady()
        {
            return MeterTimer >= 0.0f;
        }

        public MeteredPart(double meterLength, double cooldownLength, finalProject.Part.SubPart[] subParts)
            : base(subParts)
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
