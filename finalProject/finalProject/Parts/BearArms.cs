using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using System.Drawing;
using GameConstructLibrary;

namespace finalProject.Parts
{
    public class BearArms : CooldownPart
    {

        public static GameTip mTip = new GameTip(
            new string[] 
                {
                    "Using bear arms will attack creatures in front of you."
                },
            10.0f);

        protected override GameTip Tip
        {
            get
            {
                return mTip;
            }
        }

        protected const int AttackDamage = 1;
        protected const float Range = 0.75f;

        protected const double AnimationLength = 1.0f;
        protected double mAnimationTimer = -1.0f;

        protected const float DamageImpulseMultiplier = 70.0f;

        protected bool mActive = false;



        public BearArms()
            : base(
                1.1f,
                new Part.SubPart[] {
                    new SubPart(
                        new AnimateModel("bear_leftArm", "stand"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.ArmLeft1Cap, 
                            Creature.PartBone.ArmLeft2Cap,
                            Creature.PartBone.ArmLeft3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromYawPitchRoll(10.09499f, 0.0837759f, -2.638938f),
                        new Vector3(3.0f)
                    ),
                    new SubPart(
                        new AnimateModel("bear_rightArm", "stand"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.ArmRight1Cap, 
                            Creature.PartBone.ArmRight2Cap,
                            Creature.PartBone.ArmRight3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromYawPitchRoll(1.675516f, 0.502655f, 0.3769911f),
                        new Vector3(3.0f)
                    )
                },
                false,
                new Sprite("BearArmsIcon")
            )
        { }

        protected bool WithinRange(Creature creature)
        {
            float distance = (creature.Position - Creature.Position).Length();
            distance -= creature.CharacterController.BodyRadius + Creature.CharacterController.BodyRadius;
            if (distance < Range)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected List<Creature> FindTargets()
        {
            List<Creature> targets = new List<Creature>();
            foreach (Creature creature in Creature.Sensor.CollidingCreatures)
            {
                if (creature != Creature && Creature.Sensor.CanSee(creature) && WithinRange(creature))
                {
                    targets.Add(creature);
                }
            }
            return targets;
        }

        protected override void UseCooldown(Vector3 direction)
        {
            List<Creature> targets = FindTargets();

            if (targets.Count == 0 && direction.Length() < 0.9f || direction.Length() > 1.1f)
            {
                base.Reset();
            }

            PlayAnimation("attack", true, false);
            foreach (Creature creature in targets)
            {
                Vector3 impulseVector = Vector3.Normalize(creature.Position - Creature.Position);
                impulseVector.Y = 0.3f;
                impulseVector.Normalize();
                impulseVector *= DamageImpulseMultiplier;
                //creature.Entity.LinearVelocity = impulseVector;
                creature.Entity.ApplyLinearImpulse(ref impulseVector);
                creature.AllowImpulse();

                creature.Damage(AttackDamage, Creature);
            }

            mActive = true;
            mCanAnimate = false;
            mAnimationTimer = AnimationLength;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            if (mAnimationTimer > 0.0f)
            {
                mAnimationTimer -= time.ElapsedGameTime.TotalSeconds;
                if (mAnimationTimer < 0.0f)
                {
                    mActive = false;
                    mCanAnimate = true;
                }
            }
        }

        public override void FinishUse(Vector3 direction) { }

        public override void Cancel() { }

    }
}
