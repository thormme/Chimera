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

namespace finalProject.Parts
{
    class RhinoHead : CooldownPart
    {
        private const double RunLength = 5.0f;
        private const float SpeedBoost = 10.0f;
        private const float DamageMultiplier = 4.0f;

        private double mRunTimer = -1.0f;
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
                }
            )
        {
        }

        public override void Update(GameTime time)
        {
            if (mRunTimer > 0.0f)
            {
                //foreach (IGameObject gameObject in Creature.CollidingObjects)
                //{
                //    Creature creature = gameObject as Creature;
                //    if (creature != null)
                //    {
                //        Vector3 velocityDifference = Creature.Entity.LinearVelocity - creature.Entity.LinearVelocity;
                //        creature.Damage((int)(velocityDifference.Length() * DamageMultiplier), Creature);
                //    }
                //}

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
                    Creature.CharacterController.HorizontalMotionConstraint.Speed = OldSpeed;
                }
            }

            base.Update(time);
        }

        protected override void UseCooldown(Vector3 direction)
        {
            if (!Creature.CharacterController.SupportFinder.HasTraction)
            {
                CooldownTimer = -1.0f;
                return;
            }

            NewSpeed = Creature.CharacterController.HorizontalMotionConstraint.Speed + SpeedBoost;
            mRunTimer = RunLength;
            Creature.Stun(RunLength, Creature.Forward);
            OldSpeed = Creature.CharacterController.HorizontalMotionConstraint.Speed;
            Creature.CharacterController.HorizontalMotionConstraint.Speed = NewSpeed;
        }

        public override void FinishUse(Vector3 direction)
        {
        }

        public override void Reset()
        {
            base.Reset();

            mRunTimer = -1.0f;
            if (Creature != null)
            {
                Creature.CharacterController.HorizontalMotionConstraint.Speed = OldSpeed;
            }
        }

        public override void InitialCollisionDetected(BEPUphysics.Collidables.MobileCollidables.EntityCollidable sender, BEPUphysics.Collidables.Collidable other, BEPUphysics.NarrowPhaseSystems.Pairs.CollidablePairHandler collisionPair)
        {
            base.InitialCollisionDetected(sender, other, collisionPair);

            if (other.Tag is CharacterSynchronizer && mRunTimer > 0.0f)
            {
                float totalImpulse = 0;
                foreach (ContactInformation c in collisionPair.Contacts)
                {
                    totalImpulse += c.NormalImpulse;
                }

                Creature creature = (other.Tag as CharacterSynchronizer).body.Tag as Creature;
                creature.Damage((int)(totalImpulse / DamageMultiplier), Creature);
            }
        }
    }
}
