using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities.Prefabs;
using GraphicsLibrary;
using GameConstructLibrary;
using BEPUphysics.BroadPhaseEntries;
using Utility;
using BEPUphysics;
using BEPUphysics.CollisionTests;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;

namespace finalProject.Parts
{
    class RhinoHead : CooldownPart
    {
        private const double RunLength = 2.0f;
        private const float SpeedBoost = 10.0f;
        private const float DamageMultiplier = 4.0f;

        private double mRunTimer = -1.0f;
        private double mChargeAnimationTime = 5.0f;
        private float NewSpeed;
        private float OldSpeed;

        public RhinoHead()
            : base(
                10.0,
                new Part.SubPart[] {
                    new SubPart(
                        new AnimateModel("rhino_head", "stand"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.HeadCenterCap,
                            Creature.PartBone.HeadLeftCap,
                            Creature.PartBone.HeadRightCap
                        },
                        new Vector3(0.0f),
                        Matrix.CreateFromYawPitchRoll(0, 0, 0),
                        new Vector3(1.0f)
                    )
                },
                false
            )
        {
        }

        public override void Update(GameTime time)
        {
            if (mRunTimer > 0.0f)
            {
                Func<BroadPhaseEntry, bool> filter = (bfe) => ((!(bfe.Tag is Sensor)) && (!(bfe.Tag is CharacterSynchronizer)));
                RayCastResult result;
                if (Utils.FindWall(Creature.Position, Creature.Forward, filter, Creature.World.Space, 2.0f * Creature.CharacterController.BodyRadius, out result))
                //if (Utils.FindCliff(Creature.Position, Creature.Forward, filter, Creature.World.Space, 4.0f * (float)time.ElapsedGameTime.TotalSeconds * Creature.Entity.LinearVelocity.Length() + Creature.CharacterController.BodyRadius))
                {
                    Creature.Stun();
                    mRunTimer = -1.0f;
                }

                mRunTimer -= time.ElapsedGameTime.TotalSeconds;
                if (mRunTimer < 0.0f)
                {
                    Stop();
                    PlayAnimation("stand", true);
                }
                else if (mRunTimer < mChargeAnimationTime)
                {
                    PlayAnimation("charge", true);
                }
            }

            base.Update(time);
        }

        protected override void UseCooldown(Vector3 direction)
        {
            if (!Creature.CharacterController.SupportFinder.HasTraction)
            {
                base.Reset();
                return;
            }

            OldSpeed = NewSpeed = Creature.CharacterController.HorizontalMotionConstraint.Speed + SpeedBoost;
            Vector2 moveDir = new Vector2(Creature.Forward.X, Creature.Forward.Z);
            moveDir.Normalize();
            Creature.Move(moveDir);
            Creature.Immobilized = true;
            Creature.Silenced = true;
            OldSpeed = Creature.CharacterController.HorizontalMotionConstraint.Speed;
            Creature.CharacterController.HorizontalMotionConstraint.Speed = NewSpeed;
            mRunTimer = RunLength;
        }

        protected void Stop()
        {
            Creature.CharacterController.HorizontalMotionConstraint.Speed = OldSpeed;
            Creature.Immobilized = false;
            Creature.Silenced = false;
        }

        public override void FinishUse(Vector3 direction)
        {
        }

        public override void TryPlayAnimation(string animationName, bool isSaturated)
        {
            if (mRunTimer < 0.0f && animationName != "jump")
            {
                PlayAnimation(animationName, isSaturated);
            }
        }

        public override void Reset()
        {
            base.Reset();
            Cancel();
        }

        public override void Cancel()
        {
            if (Creature != null && mRunTimer > 0.0f)
            {
                Stop();
            }
            mRunTimer = -1.0f;
        }

        public override void InitialCollisionDetected(EntityCollidable sender, Collidable other, CollidablePairHandler collisionPair)
        {
            base.InitialCollisionDetected(sender, other, collisionPair);

            if (other.Tag is CharacterSynchronizer && mRunTimer > 0.0f)
            {
                float totalImpulse = 0;
                foreach (ContactInformation c in collisionPair.Contacts)
                {
                    totalImpulse += c.NormalImpulse;
                }

                System.Console.WriteLine(totalImpulse);
                Creature creature = (other.Tag as CharacterSynchronizer).body.Tag as Creature;
                if (totalImpulse > 150.0f)
                {
                    creature.Damage(2, Creature);
                }
                else if (totalImpulse > 45.0f)
                {
                    creature.Damage(1, Creature);
                }
            }
        }
    }
}
