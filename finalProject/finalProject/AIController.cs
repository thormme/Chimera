using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameConstructLibrary;

namespace finalProject
{
    /// <summary>
    /// Enum used with StateTimer to control durdling.
    /// </summary>
    enum DurdleState
    {
        Idol,
        Move,
        Wait
    }

    /// <summary>
    /// Controller used to make AIs for creatures.
    /// </summary>
    public class AIController : Controller
    {
        #region Fields

        protected const int MaxDurdleMoveTime = 4;
        protected const int MaxDurdleWaitTime = 4;

        protected Creature mTargetCreature;
        protected Vector3 mTargetPosition;
        protected bool mFollowPosition;
        protected bool mFlee;

        private StateTimer<DurdleState> mDurdleTimer;

        #endregion

        #region Public Methods

        public AIController()
        {
            mDurdleTimer = new StateTimer<DurdleState>(DurdleState.Idol);
            ResetAIState();
        }

        /// <summary>
        /// Decides which of the states the controller is in and calls the appropriate update function.
        /// </summary>
        /// <param name="time">The game time.</param>
        /// <param name="collidingCreatures">The creatures this creature knows about from its radial sensor.</param>
        public override void Update(GameTime time, List<Creature> collidingCreatures)
        {
            if (mCreature.Incapacitated)
            {
                return;
            }

            mDurdleTimer.Update(time);
            if (mTargetCreature != null)
            {
                if (mFlee)
                {
                    FleeUpdate(time);
                }
                else
                {
                    FollowCreatureUpdate(time);
                }
            }
            else if (mFollowPosition)
            {
                FollowPositionUpdate(time);
            }
            else if (mDurdleTimer.NewState() == DurdleState.Move)
            {
                DurdleMoveUpdate(time);
            }
            else if (mDurdleTimer.NewState() == DurdleState.Wait)
            {
                DurdleWaitUpdate(time);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Orders the creature to durdle around.
        /// </summary>
        protected virtual void DurdleOrder()
        {
            if (mDurdleTimer.State == DurdleState.Idol)
            {
                ResetAIState();
                mDurdleTimer.Loop();
                mDurdleTimer.Next();
            }
        }

        /// <summary>
        /// Orders the creature to go to the position.
        /// </summary>
        /// <param name="position">The position to go to.</param>
        protected virtual void FollowOrder(Vector3 position)
        {
            ResetAIState();
            mFollowPosition = true;
            mTargetPosition = position;
            MoveTo(position);
        }

        /// <summary>
        /// Orders the creature to follow the specified creature.
        /// </summary>
        /// <param name="creature">The creature to follow.</param>
        protected virtual void FollowOrder(Creature creature)
        {
            ResetAIState();
            mTargetCreature = creature;
            mFlee = false;
            MoveTo(mTargetCreature.Position);
        }

        /// <summary>
        /// Orders the creature to flee from the specified creature.
        /// </summary>
        /// <param name="creature">The creature from which to flee.</param>
        protected virtual void FleeOrder(Creature creature)
        {
            ResetAIState();
            mTargetCreature = creature;
            mFlee = true;
            MoveFrom(mTargetCreature.Position);
        }

        /// <summary>
        /// Orders the creature to stop acting.
        /// </summary>
        protected virtual void StopOrder()
        {
            ResetAIState();
            ResetPart();
            StopMoving();
        }

        /// <summary>
        /// Resets the state of the AI.
        /// </summary>
        protected virtual void ResetAIState()
        {
            mDurdleTimer.State = DurdleState.Idol;
            mDurdleTimer.ResetNewState();
            mTargetCreature = null;
            mFollowPosition = false;
        }

        /// <summary>
        /// Called in update when the durdle move state begins.
        /// </summary>
        /// <param name="time">The game time.</param>
        protected virtual void DurdleMoveUpdate(GameTime time)
        {
            mDurdleTimer.NextIn(Rand.NextFloat(MaxDurdleMoveTime));

            Vector3 newDirection = new Vector3(Rand.NextFloat(2.0f) - 1.0f, 0.0f, Rand.NextFloat(2.0f) - 1.0f);
            MoveCreature(newDirection);

            mDurdleTimer.ResetNewState();
        }

        /// <summary>
        /// Called in update when the durdle wait state begins.
        /// </summary>
        /// <param name="time">The game time.</param>
        protected virtual void DurdleWaitUpdate(GameTime time)
        {
            mDurdleTimer.NextIn(Rand.NextFloat(MaxDurdleWaitTime));
            mDurdleTimer.ResetNewState();
            StopMoving();
        }

        /// <summary>
        /// Called in update during the follow position state.
        /// </summary>
        /// <param name="time">The game time.</param>
        protected virtual void FollowPositionUpdate(GameTime time)
        {
            FollowUpdate(time, mTargetPosition);
        }

        /// <summary>
        /// Called in update during the follow creature state.
        /// </summary>
        /// <param name="time">The game time.</param>
        protected virtual void FollowCreatureUpdate(GameTime time)
        {
            FollowUpdate(time, mTargetCreature.Position);
        }

        /// <summary>
        /// Called in update by FollowCreatureUpdate and FollowPositionUpdate.
        /// </summary>
        /// <param name="time">The game time.</param>
        /// <param name="position">The position to follow.</param>
        protected virtual void FollowUpdate(GameTime time, Vector3 position)
        {
            Vector3 direction = position - mCreature.Position;
            MoveCreature(direction);
            UsePartUpdate(direction, time);
        }

        /// <summary>
        /// Called in update during the flee state.
        /// </summary>
        /// <param name="time"></param>
        protected virtual void FleeUpdate(GameTime time)
        {
            Vector3 direction = mCreature.Position - mTargetCreature.Position;
            MoveCreature(direction);
            UsePartUpdate(direction, time);
        }

        /// <summary>
        /// Called when the AI wants to use the creature's part.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="time"></param>
        protected virtual void UsePartUpdate(Vector3 direction, GameTime time)
        {
            ChoosePart().Use(direction);
        }

        /// <summary>
        /// Resets the creature's part. Called in StopOrder.
        /// </summary>
        protected virtual void ResetPart()
        {
            ChoosePart().Reset();
        }

        /// <summary>
        /// Chooses a part from the creature to use.
        /// </summary>
        /// <returns>The part to use.</returns>
        protected virtual Part ChoosePart()
        {
            return mCreature.PartAttachments[0].Part;
        }

        /// <summary>
        /// Moves in the direction of the specified position.
        /// </summary>
        /// <param name="position">The position towards which to move.</param>
        protected virtual void MoveTo(Vector3 position)
        {
            Vector3 direction = position - mCreature.Position;
            MoveCreature(direction);
        }

        /// <summary>
        /// Moves away from the specified position.
        /// </summary>
        /// <param name="position">The position from which to move away.</param>
        protected virtual void MoveFrom(Vector3 position)
        {
            Vector3 direction = mCreature.Position - position;
            MoveCreature(direction);
        }

        /// <summary>
        /// Moves the creature in the specified direction.
        /// </summary>
        /// <param name="direction">The direction in which to move.</param>
        protected virtual void MoveCreature(Vector3 direction)
        {
            Vector2 dir = new Vector2(direction.X, direction.Z);
            dir.Normalize();
            mCreature.Move(dir);
        }

        /// <summary>
        /// Makes the creature stop moving.
        /// </summary>
        protected virtual void StopMoving()
        {
            mCreature.Move(Vector2.Zero);
        }

        #endregion
    }
}
