﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace finalProject
{
    public abstract class DutyCyclePart : CooldownPart
    {
        private double mDutyLength;
        private double mDutyTimer = -1.0f;

        protected double DutyCycle
        {
            private set
            {
                mDutyLength = value * CooldownLength;
            }
            get
            {
                return mDutyLength / CooldownLength;
            }
        }

        public DutyCyclePart(double dutyCycle, double cooldownLength, SubPart[] subParts)
            : base(cooldownLength, subParts)
        {
            DutyCycle = dutyCycle;
            Reset();
        }

        protected override void UseCooldown(Vector3 direction)
        {
            if (mDutyTimer < 0.0f)
            {
                mDutyTimer = mDutyLength;
                BeginDuty(direction);
            }
        }

        public override void FinishUse(Vector3 direction)
        {
            if (mDutyTimer > 0.0f)
            {
                CooldownTimer = (mDutyLength - mDutyTimer) * (CooldownLength - mDutyLength) / mDutyLength;
                mDutyTimer = -1.0f;
                EndDuty();
            }
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            if (mDutyTimer >= 0.0f)
            {
                mDutyTimer -= time.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                EndDuty();
            }
        }

        protected override void Reset()
        {
            base.Reset();

            mDutyTimer = -1.0f;
        }

        protected abstract void BeginDuty(Vector3 direction);

        protected abstract void EndDuty();
    }
}