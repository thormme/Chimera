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
        private double mJumpTimer = -1.0f;

        public override void Damage(int damage, Creature source)
        {
            if (source != null)
            {
                FollowOrder(source);
            }
        }

        protected virtual bool WithinStompRange(Creature creature)
        {
            if (mCreature.Position.Y < creature.Position.Y)
            {
                return false;
            }

            float distance = (creature.Position - mCreature.Position).Length();
            if (distance > creature.CharacterController.BodyRadius + mCreature.CharacterController.BodyRadius)
            {
                return false;
            }

            return true;
        }

        protected override void UsePartUpdate(GameTime time)
        {
            if (State == AIState.FollowCreature && WithinStompRange(mTargetCreature))
            {
                Vector3 downVector = new Vector3(0.0f, -1.0f, 0.0f);
                Ray downRay = new Ray(mCreature.Position, downVector);
                Func<BroadPhaseEntry, bool> filter = (bfe) => (!(bfe.Tag is Sensor) && bfe.Tag != mCreature);
                RayCastResult result = new RayCastResult();
                mCreature.World.Space.RayCast(downRay, filter, out result);
                if (result.HitObject.Tag == mTargetCreature)
                {
                    Part part = ChoosePart();
                    part.Use(downVector);
                    part.FinishUse(downVector);
                }
            }

            if (mUsingPart)
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

                KangarooLegs kLegs = (ChoosePart() as KangarooLegs);
                if (mJumpTimer > kLegs.FullJumpTime)
                {
                    mJumpTimer += time.ElapsedGameTime.TotalSeconds;
                    if (mJumpTimer / kLegs.FullJumpTime > targetDirection.Length() / mCreature.Sensor.Radius)
                    {
                        kLegs.FinishUse(targetDirection);
                        mUsingPart = false;
                    }
                }
                else
                {
                    ChoosePart().Use(targetDirection);
                }
            }
        }
    }
}
