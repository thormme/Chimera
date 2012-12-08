using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using Microsoft.Xna.Framework;

namespace finalProject.AI
{
    class PenguinAI : PassiveAI
    {
        protected const float SlideSlope = 8; // degrees
        protected const int SlideFrames = 10;
        protected const double WaitLength = 2.0f;
        protected const float WaitDistance = 0.5f;
        //protected const double SlideDot = Math.Cos((double)MathHelper.ToRadians(SlideSlope));

        protected bool mClimbing = false;
        protected int mFlatFrames = 0;
        protected int mSteepFrames = 0;
        protected double mWaitTimer = WaitLength;
        protected Vector3 mWaitPosition = Vector3.Zero;
        protected bool mWaited = false;
        protected bool mLost = false;

        protected override void DurdleMoveUpdate(GameTime time)
        {
            Vector3 steepestNormal = Vector3.Zero;
            float mostDegrees = 0.0f;
            foreach (SupportContact support in mCreature.CharacterController.SupportFinder.Supports)
            {
                Vector3 normal = -support.Contact.Normal;
                normal.Normalize();

                double dot = Vector3.Dot(Vector3.Up, normal);
                float degrees = MathHelper.ToDegrees((float)Math.Acos(dot));
                if (degrees > mostDegrees)
                {
                    steepestNormal = normal;
                    mostDegrees = degrees;
                }
            }

            mWaitTimer -= time.ElapsedGameTime.TotalSeconds;
            if (mWaitTimer < 0.0f)
            {
                mWaited = false;
                if ((mCreature.Position - mWaitPosition).Length() < WaitDistance)
                {
                    //ChoosePart().Use(mCreature.Forward);
                    mCreature.Jump();
                    FinishUsePart();
                    mClimbing = false;
                    mWaited = true;
                    if (mostDegrees < 1.0f)
                    {
                        mLost = true;
                    }
                }
                mWaitPosition = mCreature.Position;
                mWaitTimer = WaitLength;
            }

            //
            //System.Console.WriteLine(mostDegrees);

            Vector3 moveDirection = Vector3.Zero;
            if (!mUsingPart && mostDegrees > 0.0f)
            {
                moveDirection = Vector3.Cross(steepestNormal, Vector3.Up);
                moveDirection = Vector3.Cross(moveDirection, steepestNormal);

                MoveCreature(moveDirection);
                mLost = false;
            }

            if (mostDegrees < SlideSlope)
            {
                mSteepFrames = 0;
                ++mFlatFrames;
                if (mFlatFrames >= SlideFrames)
                {
                    if (mClimbing)
                    {
                        mCreature.UsePart(ChoosePartSlot(), mCreature.Forward);
                        mUsingPart = true;
                    }
                }
            }
            else
            {
                mFlatFrames = 0;
                ++mSteepFrames;
                if (mSteepFrames >= 10)
                {
                    mClimbing = !mUsingPart;
                }
            }
        }

        protected override void DurdleBeginMoveUpdate(GameTime time)
        {
            if (!mLost)
            {
                mDurdleTimer.NextIn(Rand.NextFloat(MaxDurdleMoveTime));
                mDurdleTimer.ResetNewState();
            }
            else
            {
                base.DurdleBeginMoveUpdate(time);
            }
        }

        protected override void DurdleBeginWaitUpdate(GameTime time)
        {
            if (!mLost)
            {
                mDurdleTimer.Next();
            }
            else
            {
                base.DurdleBeginWaitUpdate(time);
            }
        }

        protected override void UsePart(Vector3 direction)
        {
        }

        protected override void UsePartUpdate(GameTime time)
        {
        }
    }
}
