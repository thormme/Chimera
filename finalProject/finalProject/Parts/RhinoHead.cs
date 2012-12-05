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

namespace finalProject.Parts
{
    class RhinoHead : CooldownPart
    {
        //private const float GroundDamageMultiplier = 3.0f;
        //private const float AirDamageMultiplier = 1.0f;
        //private const double AttackLength = 0.6f;
        //private const double DamageStart = 0.4f;
        //private double mAttackTimer = -1.0f;

        private const double RunLength = 5.0f;
        private const float SpeedBoost = 10.0f;
        private const int DamageMultiplier = 3;

        private double mRunTimer = -1.0f;
        private float NewSpeed;
        private float OldSpeed;


        public RhinoHead()
            : base(
                10.0,
                new Part.SubPart[] {
                    new SubPart(
                        new AnimateModel("rhino_head", "walk"),
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
                foreach (IGameObject gameObject in Creature.CollidingObjects)
                {
                    Creature creature = gameObject as Creature;
                    if (creature != null)
                    {
                        Vector3 velocityDifference = Creature.Entity.LinearVelocity - creature.Entity.LinearVelocity;
                        creature.Damage((int)(velocityDifference.Length() * DamageMultiplier), Creature);
                    }
                }

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
            OldSpeed = Creature.CharacterController.HorizontalMotionConstraint.Speed;
            Creature.CharacterController.HorizontalMotionConstraint.Speed = NewSpeed;
            Creature.Stun(RunLength, Creature.Forward);
            //if (Creature.CharacterController.SupportFinder.HasTraction)
            //{
            //    Vector3 impulse = Creature.Forward * 300f;
            //    Creature.Entity.ApplyLinearImpulse(ref impulse);
            //}
            //mAttackTimer = AttackLength;


        }

        public override void FinishUse(Vector3 direction)
        {
        }

        public override void Reset()
        {
            base.Reset();

            //mAttackTimer = -1.0f;
        }
    }
}
