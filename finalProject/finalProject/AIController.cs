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

        private const int MaxDurdleMoveTime = 4;
        private const int MaxDurdleWaitTime = 4;
        private const float MaxDurdleSpeed = 1.0f;

        private Creature mTargetCreature;
        private Vector3 mTargetPosition;
        private bool mFollowPosition;
        private float mInversion;

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

            //if (!active)
            //{
            //    return;
            //}

            mDurdleTimer.Update(time);
            if (mTargetCreature != null)
            {
                MoveTo(mTargetCreature.Position, mInversion);
                Vector3 direction = mTargetCreature.Position - mCreature.Position;
                if (mInversion < 0.0f)
                {
                    direction = -direction;
                }
                mCreature.UsePart(0, direction);
            }
            else if (mFollowPosition)
            {
                MoveTo(mTargetPosition, mInversion);
                mCreature.UsePart(0, mTargetPosition - mCreature.Position);
            }
            else if (mDurdleTimer.NewState() == DurdleState.Move)
            {
                // TODO: For some reason this seems to go into one quadrant.
                mDurdleTimer.NextIn(Rand.NextFloat(MaxDurdleMoveTime));

                Vector3 newDirection = new Vector3(Rand.NextFloat(2.0f) - 1.0f, 0.0f, Rand.NextFloat(2.0f) - 1.0f);
                MoveCreature(newDirection, Rand.NextFloat(MaxDurdleSpeed));

                mDurdleTimer.ResetNewState();
            }
            else if (mDurdleTimer.NewState() == DurdleState.Wait)
            {
                mDurdleTimer.NextIn(Rand.NextFloat(MaxDurdleWaitTime));
                mDurdleTimer.ResetNewState();
                mCreature.Move(new Vector2(0.0f));
            }
        }

        private void MoveTo(Vector3 position, float inversion)
        {
            Vector3 direction = position - mCreature.Position;
            MoveCreature(direction, inversion);
        }

        private void MoveCreature(Vector3 direction, float inversion)
        {
            Vector2 dir = new Vector2(direction.X, direction.Z) * inversion;
            dir.Normalize();
            mCreature.Move(dir);
        }

        public virtual void Stop()
        {
            mDurdleTimer.State = DurdleState.Idol;
            mDurdleTimer.ResetNewState();
            if (mCreature != null)
            {
                mCreature.Move(Vector2.Zero);
            }
            mTargetCreature = null;
            mFollowPosition = false;
        }

        public virtual void Follow(Vector3 position)
        {
            Stop();
            mFollowPosition = true;
            mTargetPosition = position;
            MoveTo(position, 1.0f);
        }

        public virtual void Follow(Creature creature)
        {
            Stop();
            mTargetCreature = creature;
            mInversion = 1.0f;
            MoveTo(mTargetCreature.Position, mInversion);
        }

        public virtual void Run(Creature creature)
        {
            Stop();
            mTargetCreature = creature;
            mInversion = -1.0f;
            MoveTo(mTargetCreature.Position, mInversion);
        }
    }
}
