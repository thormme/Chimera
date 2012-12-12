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
using BEPUphysics.Entities;

namespace finalProject.Parts
{
    class RhinoHead : CooldownPart
    {
        private const double RunLength = 2.0f;
        private const float SpeedBoost = 10.0f;
        protected const float DamageImpulseMultiplier = 200.0f;

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
                        new Vector3(3.0f)
                    )
                },
                false,
                new Sprite("RhinoHeadIcon")
            )
        {
        }

        public override void Update(GameTime time)
        {
            
            if (mRunTimer > 0.0f)
            {
                Func<BroadPhaseEntry, bool> filter = (bfe) => ((!(bfe.Tag is Sensor)) && (!(bfe.Tag is CharacterSynchronizer)) && (!(bfe.Tag is PhysicsProp)));
                RayCastResult result;
                if (ObstacleDetector.FindWall(Creature.Position, Creature.Forward, filter, Creature.World.Space, 2.0f * Creature.CharacterController.BodyRadius, out result))
                //if (Utils.FindCliff(Creature.Position, Creature.Forward, filter, Creature.World.Space, 4.0f * (float)time.ElapsedGameTime.TotalSeconds * Creature.Entity.LinearVelocity.Length() + Creature.CharacterController.BodyRadius))
                {
                    Creature.Stun();
                    mRunTimer = -1.0f;
                }

                //Console.WriteLine(mRunTimer);
                mRunTimer -= time.ElapsedGameTime.TotalSeconds;
                if (mRunTimer < 0.0f)
                {
                    Stop();
                    PlayAnimation("stand", true, true);
                    mCanAnimate = true;
                }
                else if (mRunTimer < mChargeAnimationTime)
                {
                    PlayAnimation("charge", false, true);
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

            if (moveDir != Vector2.Zero)
            {
                moveDir.Normalize();
            }
            Creature.Move(moveDir);
            Creature.Immobilized = true;
            Creature.Silenced = true;
            OldSpeed = Creature.CharacterController.HorizontalMotionConstraint.Speed;
            Creature.CharacterController.HorizontalMotionConstraint.Speed = NewSpeed;
            mRunTimer = RunLength;
            mCanAnimate = false;
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

            Console.WriteLine(other.Tag + " " + mRunTimer);
            if ((other.Tag is CharacterSynchronizer || other.Tag is PhysicsProp) && mRunTimer > 0.0f)
            {
                float totalImpulse = 0;
                foreach (ContactInformation c in collisionPair.Contacts)
                {
                    totalImpulse += c.NormalImpulse;
                }

                //Console.WriteLine(totalImpulse);
                    
                int damage = 0;
                if (totalImpulse > 150.0f)
                {
                    damage = 2;
                }
                else if (totalImpulse > 45.0f)
                {
                    damage = 1;
                }

                Entity hitEntity;
                if (other.Tag is CharacterSynchronizer)
                {
                    hitEntity = (other.Tag as CharacterSynchronizer).body;
                }
                else
                {
                    hitEntity = (other.Tag as PhysicsProp).Entity;
                }

                Vector3 impulseVector = Vector3.Zero;
                Vector3 impulseDirection = hitEntity.Position - Creature.Position;
                if (impulseDirection != Vector3.Zero)
                {
                    impulseVector = Vector3.Normalize(impulseDirection);
                }

                impulseVector.Y = 1.0f;
                impulseVector.Normalize();
                impulseVector *= damage * DamageImpulseMultiplier;
                hitEntity.ApplyLinearImpulse(ref impulseVector);

                if (other.Tag is CharacterSynchronizer)
                {
                    ((other.Tag as CharacterSynchronizer).body.Tag as Creature).Damage(damage, Creature);
                }
            }
        }
    }
}
