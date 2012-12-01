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
        protected const int AttackDamage = 35;
        protected const float Range = 0.75f;

        public BearArms()
            : base(
                1.1f,
                new Part.SubPart[] {
                    new SubPart(
                        new InanimateModel("sphere"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.ArmLeft1Cap, 
                            Creature.PartBone.ArmLeft2Cap,
                            Creature.PartBone.ArmLeft3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromQuaternion(new Quaternion()),
                        new Vector3(0.25f, 0.25f, 0.25f)
                    ),
                    new SubPart(
                        new InanimateModel("sphere"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.ArmRight1Cap, 
                            Creature.PartBone.ArmRight2Cap,
                            Creature.PartBone.ArmRight3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromQuaternion(new Quaternion()),
                        new Vector3(0.25f, 0.25f, 0.25f)
                    )
                }
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
            foreach (Creature creature in targets)
            {
                creature.Damage(AttackDamage);
            }
        }

        public override void FinishUse(Vector3 direction) { }
    }
}
