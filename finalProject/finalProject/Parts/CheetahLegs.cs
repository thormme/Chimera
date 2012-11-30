using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphicsLibrary;
using Microsoft.Xna.Framework;

namespace finalProject.Parts
{

    public class CheetahLegs : MeteredPart
    {
        private const float RunSpeed = 30.0f;

        private float mCreatureSpeed;

        public CheetahLegs()
            : base(
                2.0f,
                4.0f,
                new Part.SubPart[] {
                    new SubPart(
                        new InanimateModel("sphere"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.LegFrontLeft1Cap,
                            Creature.PartBone.LegFrontLeft2Cap,
                            Creature.PartBone.LegFrontLeft3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromQuaternion(new Quaternion()),
                        new Vector3(0.25f, 0.25f, 0.25f)
                    ),
                    new SubPart(
                        new InanimateModel("sphere"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.LegFrontRight1Cap,
                            Creature.PartBone.LegFrontRight2Cap,
                            Creature.PartBone.LegFrontRight3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromQuaternion(new Quaternion()),
                        new Vector3(0.25f, 0.25f, 0.25f)
                    ),
                    new SubPart(
                        new InanimateModel("sphere"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.LegRearLeft1Cap,
                            Creature.PartBone.LegRearLeft2Cap,
                            Creature.PartBone.LegRearLeft3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromQuaternion(new Quaternion()),
                        new Vector3(0.25f, 0.25f, 0.25f)
                    ),
                    new SubPart(
                        new InanimateModel("sphere"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.LegRearRight1Cap,
                            Creature.PartBone.LegRearRight2Cap,
                            Creature.PartBone.LegRearRight3Cap
                        },
                        new Vector3(),
                        Matrix.CreateFromQuaternion(new Quaternion()),
                        new Vector3(0.25f, 0.25f, 0.25f)
                    )
                }
            )
        { }
                
        protected override void UseMeter(Vector3 direction)
        {
            Creature.CharacterController.HorizontalMotionConstraint.Speed = RunSpeed;
        }

        protected override void FinishUseMeter()
        {
            Creature.CharacterController.HorizontalMotionConstraint.Speed = mCreatureSpeed;
        }

        public override Creature Creature
        {
            protected get
            {
                return base.Creature;
            }
            set
            {
                base.Creature = value;
                mCreatureSpeed = value.CharacterController.HorizontalMotionConstraint.Speed;
            }
        }
    }
}
