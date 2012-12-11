using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameConstructLibrary;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics;
using Utility;

namespace finalProject
{
    /// <summary>
    /// Enum used with StateTimer to control durdling.
    /// </summary>
    public enum DurdleState
    {
        Idol,
        Move,
        Wait
    }

    public enum AIState
    {
        Idol,
        Stop,
        Durdle,
        FollowPosition,
        FollowCreature,
        FleeCreature
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
        protected Vector3 mDurdleDirection;

        protected Vector3 mMoveDirection;

        protected bool mUsingPart = false;
        protected Vector3 mUsePartDirection;

        protected StateTimer<DurdleState> mDurdleTimer;

        #endregion

        #region Protected Properties

        protected AIState State
        {
            get;
            set;
        }

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
            if (State == AIState.Durdle)
            {
                if (mDurdleTimer.NewState() == DurdleState.Move)
                {
                    DurdleBeginMoveUpdate(time);
                }
                else if (mDurdleTimer.State == DurdleState.Move)
                {
                    DurdleMoveUpdate(time);
                }
                else if (mDurdleTimer.NewState() == DurdleState.Wait)
                {
                    DurdleBeginWaitUpdate(time);
                }
                else if (mDurdleTimer.State == DurdleState.Wait)
                {
                    DurdleWaitUpdate(time);
                }
            }
            else if (State == AIState.FleeCreature)
            {
                FleeCreatureUpdate(time);
            }
            else if (State == AIState.FollowCreature)
            {
                FollowCreatureUpdate(time);
            }
            else if (State == AIState.FollowPosition)
            {
                FollowPositionUpdate(time);
            }

            if (!NoControl)
            {
                MoveUpdate(time);
                UsePartUpdate(time);
            }
        }

        #endregion

        #region Orders

        /// <summary>
        /// Orders the creature to durdle around.
        /// </summary>
        protected virtual void DurdleOrder()
        {
            if (State != AIState.Durdle)
            {
                ResetAIState();
                State = AIState.Durdle;
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
            if (!(State == AIState.FollowPosition && position.Equals(mTargetPosition)))
            {
                ResetAIState();
                mFollowPosition = true;
                mTargetPosition = position;
                State = AIState.FollowPosition;
                MoveTo(position);
            }
        }

        /// <summary>
        /// Orders the creature to follow the specified creature.
        /// </summary>
        /// <param name="creature">The creature to follow.</param>
        protected virtual void FollowOrder(Creature creature)
        {
            if (!(State == AIState.FollowCreature && creature == mTargetCreature))
            {
                ResetAIState();
                mTargetCreature = creature;
                State = AIState.FollowCreature;
                MoveTo(mTargetCreature.Position);
            }
        }

        /// <summary>
        /// Orders the creature to flee from the specified creature.
        /// </summary>
        /// <param name="creature">The creature from which to flee.</param>
        protected virtual void FleeOrder(Creature creature)
        {
            if (!(State == AIState.FleeCreature && creature == mTargetCreature))
            {
                ResetAIState();
                mTargetCreature = creature;
                State = AIState.FleeCreature;
                MoveFrom(mTargetCreature.Position);
            }
        }

        /// <summary>
        /// Orders the creature to stop acting.
        /// </summary>
        protected virtual void StopOrder()
        {
            ResetAIState();
            State = AIState.Stop;
            StopMoving();
        }

        #endregion

        #region Updates

        protected virtual void MoveUpdate(GameTime time)
        {
            if (mMoveDirection.Length() > 0.2f)
            {
                Func<BroadPhaseEntry, bool> filter = (bfe) => ((!(bfe.Tag is Sensor)) && (!(bfe.Tag is CharacterSynchronizer)));

                float distance = /*(float)time.ElapsedGameTime.TotalSeconds * mCreature.Entity.LinearVelocity.Length() * */5.0f + mCreature.CharacterController.BodyRadius;
                RayCastResult result;

                if (State != AIState.Durdle && ObstacleDetector.FindWall(mCreature.Position, mCreature.Position, filter, mCreature.World.Space, distance, out result))
                {
                    mMoveDirection.Y = 0;
                    mMoveDirection.Normalize();
                    Vector3 cross = Vector3.Cross(mMoveDirection, result.HitData.Normal);
                    Vector3 wallparallel = Vector3.Cross(cross, result.HitData.Normal);
                    wallparallel.Y = 0;
                    wallparallel.Normalize();

                    if (Vector3.Dot(mMoveDirection, wallparallel) >= 0.0f)
                    {
                        mMoveDirection = wallparallel;
                    }
                    else
                    {
                        mMoveDirection = -wallparallel;
                    }
                }

                Vector3 newDirection = mCreature.Forward;
                newDirection += 10.0f * (float)time.ElapsedGameTime.TotalSeconds * (mMoveDirection - mCreature.Forward);

                //if (State != AIState.Durdle && Utils.FindWall(mCreature.Position, newDirection, filter, mCreature.World.Space, distance, out result))
                //{
                //    newDirection.Y = 0;
                //    newDirection.Normalize();
                //    Vector3 cross = Vector3.Cross(newDirection, result.HitData.Normal);
                //    cross.Y = 0;
                //    cross.Normalize();

                //    if (Vector3.Dot(newDirection, cross) >= 0.0f)
                //    {
                //        newDirection = cross;
                //    }
                //    else
                //    {
                //        newDirection = -cross;
                //    }
                //}

                Vector2 dir = new Vector2(newDirection.X, newDirection.Z);
                dir.Normalize();


                if (State == AIState.Durdle)
                {
                    if (ObstacleDetector.FindWall(mCreature.Position, mMoveDirection, filter, mCreature.World.Space, distance, out result) ||
                        ObstacleDetector.FindCliff(mCreature.Position, mMoveDirection, filter, mCreature.World.Space, distance))
                    {
                        dir = Vector2.Zero;
                    }
                }
                mCreature.Move(dir);
            }
        }

        /// <summary>
        /// Called in update when the durdle move state begins.
        /// </summary>
        /// <param name="time">The game time.</param>
        protected virtual void DurdleBeginMoveUpdate(GameTime time)
        {
            mDurdleTimer.NextIn(Rand.NextFloat(MaxDurdleMoveTime));
            mDurdleTimer.ResetNewState();

            MoveCreature(new Vector3(Rand.NextFloat(2.0f) - 1.0f, 0.0f, Rand.NextFloat(2.0f) - 1.0f));
        }

        /// <summary>
        /// Called in update every frame the creature is durdling.
        /// </summary>
        /// <param name="time">The game time.</param>
        protected virtual void DurdleMoveUpdate(GameTime time) { }

        /// <summary>
        /// Called in update when the durdle wait state begins.
        /// </summary>
        /// <param name="time">The game time.</param>
        protected virtual void DurdleBeginWaitUpdate(GameTime time)
        {
            mDurdleTimer.NextIn(Rand.NextFloat(MaxDurdleWaitTime));
            mDurdleTimer.ResetNewState();

            StopMoving();
        }

        /// <summary>
        /// Called in update every frame the creature is waiting.
        /// </summary>
        /// <param name="time">The game time.</param>
        protected virtual void DurdleWaitUpdate(GameTime time) { }

        /// <summary>
        /// Called in update during the flee state.
        /// </summary>
        /// <param name="time"></param>
        protected virtual void FleeCreatureUpdate(GameTime time)
        {
            if (mTargetCreature.Incapacitated)
            {
                mTargetCreature = null;
                ResetAIState();
            }
            else
            {
                Vector3 direction = mCreature.Position - mTargetCreature.Position;
                MoveCreature(direction);
                UsePart(direction);
            }
        }

        /// <summary>
        /// Called in update during the follow creature state.
        /// </summary>
        /// <param name="time">The game time.</param>
        protected virtual void FollowCreatureUpdate(GameTime time)
        {
            if (mTargetCreature.Incapacitated)
            {
                mTargetCreature = null;
                ResetAIState();
            }
            else
            {
                FollowUpdate(time, mTargetCreature.Position);
            }
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
        /// Called in update by FollowCreatureUpdate and FollowPositionUpdate.
        /// </summary>
        /// <param name="time">The game time.</param>
        /// <param name="position">The position to follow.</param>
        protected virtual void FollowUpdate(GameTime time, Vector3 position)
        {
            Vector3 direction = position - mCreature.Position;
            MoveCreature(direction);
            UsePart(direction);
        }

        /// <summary>
        /// Called every frame. Controls the appropriate use of the creature's part.
        /// </summary>
        /// <param name="time">The game time.</param>
        protected virtual void UsePartUpdate(GameTime time)
        {
            if (mUsingPart)
            {
                int part = ChoosePartSlot();
                mCreature.UsePart(part, mUsePartDirection);
                mCreature.FinishUsePart(part, mUsePartDirection);
                mUsingPart = false;
            }
        }

        #endregion

        #region Helpers

        ///// <summary>
        ///// Resets the creature's part. Called in StopOrder.
        ///// </summary>
        //protected virtual void ResetPart()
        //{
        //    ChoosePartSlot().Reset();
        //}

        /// <summary>
        /// Chooses a part from the creature to use.
        /// </summary>
        /// <returns>The part to use.</returns>
        protected virtual int ChoosePartSlot()
        {
            return 0;
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
            mMoveDirection = direction;
            mMoveDirection.Normalize();
        }

        /// <summary>
        /// Makes the creature stop moving.
        /// </summary>
        protected virtual void StopMoving()
        {
            mCreature.Move(Vector2.Zero);
            MoveCreature(Vector3.Zero);
        }

        /// <summary>
        /// Resets the state of the AI.
        /// </summary>
        protected virtual void ResetAIState()
        {
            State = AIState.Idol;
            mDurdleTimer.State = DurdleState.Idol;
            mDurdleTimer.ResetNewState();
            mTargetCreature = null;
            mFollowPosition = false;
            if (mUsingPart)
            {
                FinishUsePart();
            }
        }

        /// <summary>
        /// Called when the AI wants to use the creature's part.
        /// </summary>
        /// <param name="direction">The direction in which to use the part.</param>
        protected virtual void UsePart(Vector3 direction)
        {
            mUsePartDirection = direction;
            mUsingPart = true;
        }

        /// <summary>
        /// Called when the AI wants to stop using the creature's part.
        /// </summary>
        protected virtual void FinishUsePart()
        {
            mCreature.FinishUsePart(ChoosePartSlot(), mCreature.Forward);
            mUsingPart = false;
        }

        #endregion
    }
}
