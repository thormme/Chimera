using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameConstructLibrary;

enum DurdleState
{
    Idol,
    Move,
    Wait
}

namespace finalProject
{
    public class AIController : Controller
    {
        KeyInputAction mGo;
        bool active = false;

        private const int MaxDurdleMoveTime = 2;
        private const int MaxDurdleWaitTime = 1;
        private const float MaxDurdleSpeed = 1.0f;

        private Creature mTargetCreature;
        private float mMagnitude;

        private StateTimer<DurdleState> mDurdleTimer;

        public AIController()
        {
            mDurdleTimer = new StateTimer<DurdleState>(DurdleState.Idol);
            Stop();
            mGo = new KeyInputAction(PlayerIndex.One, InputAction.ButtonAction.Pressed, Microsoft.Xna.Framework.Input.Keys.RightShift);
        }

        /// <summary>
        /// Tells the creature to durdle around.
        /// </summary>
        public virtual void Durdle()
        {
            if (mDurdleTimer.State == DurdleState.Idol)
            {
                Stop();
                mDurdleTimer.Loop();
                mDurdleTimer.Next();
            }
        }

        public override void Update(GameTime time, List<Creature> collidingCreatures)
        {

            if (mGo.Active)
            {
                active = true;
            }

            if (!active)
            {
                return;
            }

            mDurdleTimer.Update(time);
            if (mTargetCreature != null)
            {
                MoveTo(mTargetCreature.Position, mMagnitude);
            }

            if (mDurdleTimer.NewState() == DurdleState.Move)
            {
                // TODO: For some reason this seems to go into one quadrant.
                mDurdleTimer.NextIn(Rand.NextFloat(MaxDurdleMoveTime));

                Vector3 newDirection = new Vector3(Rand.NextFloat(2.0f) - 1.0f, 0.0f, Rand.NextFloat(2.0f) - 1.0f);
                newDirection.Normalize();
                Console.WriteLine(newDirection.X + " " + newDirection.Z);
                //newDirection *= (float)Math.Sqrt(1 - mCreature.Forward.Y * mCreature.Forward.Y);
                //newDirection.Y = mCreature.Forward.Y;
                mCreature.Forward = newDirection;
                mCreature.Move(Rand.NextFloat(MaxDurdleSpeed));

                mDurdleTimer.ResetNewState();
            }
            else if (mDurdleTimer.NewState() == DurdleState.Wait)
            {
                mDurdleTimer.NextIn(Rand.NextFloat(MaxDurdleWaitTime));
                mDurdleTimer.ResetNewState();
                mCreature.Move(0.0f);
            }
        }

        private void MoveTo(Vector3 position, float magnitude)
        {
            mCreature.Forward = Vector3.Normalize((position - mCreature.Position) * magnitude);
            mCreature.Move(Math.Abs(magnitude));
        }

        public virtual void Stop()
        {
            mDurdleTimer.State = DurdleState.Idol;
            mDurdleTimer.ResetNewState();
            if (mCreature != null)
            {
                mCreature.Move(0.0f);
            }
            mTargetCreature = null;
            mMagnitude = 0.0f;
        }

        public virtual void Follow(Vector3 position)
        {
            Stop();
            MoveTo(position, 1.0f);
        }

        public virtual void Follow(Creature creature)
        {
            Stop();
            mTargetCreature = creature;
            mMagnitude = 1.0f;
            MoveTo(mTargetCreature.Position, mMagnitude);
        }

        public virtual void Run(Creature creature)
        {
            Stop();
            mTargetCreature = creature;
            mMagnitude = -1.0f;
            MoveTo(mTargetCreature.Position, mMagnitude);
        }
    }
}
