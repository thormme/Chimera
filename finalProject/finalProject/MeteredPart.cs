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
            return MeterTimer > 0.0f;
        }

        public MeteredPart(double meterLength, double cooldownLength, finalProject.Part.SubPart[] subParts)
            : base(subParts)
        {
            MeterLength = meterLength;
            CooldownLength = cooldownLength;
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
                if (CooldownTimer < 0.0f)
                {
                    MeterTimer = MeterLength;
                }
            }
        }

        protected abstract void UseMeter(Vector3 direction);
        protected abstract void FinishUseMeter();
    }
}
