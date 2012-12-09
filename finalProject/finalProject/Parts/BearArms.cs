using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;

namespace finalProject.Parts
{
    public class BearArms : CooldownPart
    {
        protected const int AttackDamage = 1;
        protected const float Range = 0.75f;

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
                        Matrix.CreateFromQuaternion(new Quaternion()),
                        new Vector3(1.0f)
                    ),
                    new SubPart(
                        new AnimateModel("bear_rightArm", "stand"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.ArmRight1Cap, 
                            Creature.PartBone.ArmRight2Cap,
                            Creature.PartBone.ArmRight3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromQuaternion(new Quaternion()),
                        new Vector3(1.0f)
                    )
                },
                false,
                new Sprite("bearIcon")
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

            PlayAnimation("attack", true);
            foreach (Creature creature in targets)
            {
                creature.Damage(AttackDamage, Creature);
            }
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
        }

        public override void FinishUse(Vector3 direction) { }

        public override void Cancel() { }

        public override void TryPlayAnimation(string animationName, bool isSaturated)
        {
            if (animationName != "jump")
            {
                PlayAnimation(animationName, isSaturated);
            }
        }
    }
}
