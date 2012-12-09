using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics;
using GameConstructLibrary;
using BEPUphysics.BroadPhaseEntries;
using finalProject.Parts;

namespace finalProject.AI
{
    class KangarooAI : PassiveAI
    {
        private double mJumpTimer = double.MaxValue;

        public override void Damage(int damage, Creature source)
        {
            if (source != null)
            {
                FollowOrder(source);
            }
        }

        protected virtual bool WithinStompRange(Creature creature)
        {
            float myHeight = mCreature.Position.Y - mCreature.CharacterController.Body.Height / 2;
            float targetHeight = creature.Position.Y + creature.CharacterController.Body.Height / 2 + 0.1f;
            if (myHeight < targetHeight)
            {
                return false;
            }

            float distance = mCreature.CollideDistance(creature);
            if (distance > -1.0f)
            {
                return false;
            }

            return true;
        }

        protected override void UsePartUpdate(GameTime time)
        {
            bool onGround = mCreature.CharacterController.SupportFinder.HasTraction;

            if (!onGround && State == AIState.FollowCreature && WithinStompRange(mTargetCreature))
            {
                Vector3 downVector = mTargetCreature.Position - mCreature.Position;
                Ray downRay = new Ray(mCreature.Position, downVector);
                Func<BroadPhaseEntry, bool> filter = (bfe) => (!(bfe.Tag is Sensor) && bfe.Tag != mCreature);
                RayCastResult result = new RayCastResult();
                mCreature.World.Space.RayCast(downRay, filter, out result);
                if (result.HitObject.Tag is CharacterSynchronizer)
                {
                    if ((result.HitObject.Tag as CharacterSynchronizer).body.Tag == mTargetCreature)
                    {
                        int part = ChoosePartSlot();
                        mCreature.UsePart(part, downVector);
                        mCreature.FinishUsePart(ChoosePartSlot(), downVector);
                        return;
                    }
                }
                else if (result.HitObject.Tag == mTargetCreature)
                {
                    int part = ChoosePartSlot();
                    mCreature.UsePart(part, downVector);
                    mCreature.FinishUsePart(part, downVector);
                    return;
                }
                ////Console.WriteLine("Ray fail");
            }
            else if (mUsingPart && mTargetCreature == null)
            {
                mCreature.FinishUsePart(ChoosePartSlot(), mCreature.Forward);
                mUsingPart = false;
            }
            else if (mUsingPart && onGround)
            {
                Vector3 targetDirection;
                if (State == AIState.FollowCreature)
                {
                    targetDirection = mTargetCreature.Position - mCreature.Position;
                }
                else if (State == AIState.FleeCreature)
                {
                    targetDirection = mCreature.Position - mTargetCreature.Position;
                }
                else
                {
                    return;
                }

                int part = ChoosePartSlot();
                KangarooLegs kLegs = (mCreature.PartAttachments[part].Part as KangarooLegs);
                if (mJumpTimer < kLegs.FullJumpTime)
                {
                    mJumpTimer += time.ElapsedGameTime.TotalSeconds;
                    if (mJumpTimer / kLegs.FullJumpTime > targetDirection.Length() / mCreature.Sensor.Radius)
                    {
                        mCreature.FinishUsePart(part, targetDirection);
                        mUsingPart = false;
                        mJumpTimer = double.MaxValue;
                    }
                }
                else
                {
                    mCreature.UsePart(ChoosePartSlot(), targetDirection);
                    mJumpTimer = 0.0f;
                }
            }
        }

        protected override void FinishUsePart()
        { }
    }
}
