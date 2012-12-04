using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities.Prefabs;
using GraphicsLibrary;
using GameConstructLibrary;

namespace finalProject.Parts
{
    class RhinoHead : CooldownPart
    {
        private const float GroundDamageMultiplier = 3.0f;
        private const float AirDamageMultiplier = 1.0f;
        private const double AttackLength = 0.6f;
        private const double DamageStart = 0.4f;
        private double mAttackTimer = -1.0f;


        public RhinoHead()
            : base(
                2.0,
                new Part.SubPart[] {
                    new SubPart(
                        new AnimateModel("rhino_head", "charge"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.HeadCenterCap,
                            Creature.PartBone.HeadLeftCap,
                            Creature.PartBone.HeadRightCap
                        },
                        new Vector3(0.0f),
                        Matrix.CreateFromYawPitchRoll(-MathHelper.PiOver2, -MathHelper.PiOver4, 0),
                        new Vector3(4.0f)
                    )
                }
            )
        {
            //(mRenderable as AnimateModel).PlayAnimation("Take 001");
        }

        public override void Update(GameTime time)
        {
            foreach (SubPart part in SubParts)
            {
                (part.Renderable as AnimateModel).Update(time);
            }

            if (mAttackTimer > 0.0f)
            {
                float multiplier = Creature.CharacterController.SupportFinder.HasTraction ? GroundDamageMultiplier : AirDamageMultiplier;
                mAttackTimer -= time.ElapsedGameTime.TotalSeconds;
                if (mAttackTimer < DamageStart)
                {
                    foreach (IGameObject gameObject in Creature.CollidingObjects)
                    {
                        Creature otherCreature = gameObject as Creature;
                        if (otherCreature != null)
                        {
                            Vector3 velocityDifference = Creature.Entity.LinearVelocity - otherCreature.Entity.LinearVelocity;
                            otherCreature.Damage((int)(velocityDifference.Length() * multiplier), Creature);
                        }
                    }
                }
            }

            base.Update(time);
        }

        protected override void UseCooldown(Vector3 direction)
        {
            if (Creature.CharacterController.SupportFinder.HasTraction)
            {
                Vector3 impulse = Creature.Forward * 300f;
                Creature.Entity.ApplyLinearImpulse(ref impulse);
            }
            mAttackTimer = AttackLength;
        }

        public override void FinishUse(Vector3 direction)
        {
        }

        public override void Reset()
        {
            base.Reset();

            mAttackTimer = -1.0f;
        }
    }
}
