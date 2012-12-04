using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;

namespace finalProject.Parts
{
    public class FrilledLizardHead : MeteredPart
    {
        public const int IntimidationIncrease = 5;
        public bool Active = false;

        public FrilledLizardHead()
            : base(
                3.0f,
                5.0f,
                new Part.SubPart[] {
                    new SubPart(
                        new AnimateModel("lizard_head", "stand"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.HeadCenterCap, 
                            Creature.PartBone.HeadLeftCap,
                            Creature.PartBone.HeadRightCap
                        },
                        new Vector3(),
                        Matrix.CreateFromQuaternion(new Quaternion()),
                        new Vector3(4.0f)
                    )
                }
            )
        { }

        protected override void UseMeter(Vector3 direction)
        {
            if (!Active)
            {
                Active = true;
                Creature.Intimidation += IntimidationIncrease;
            }
        }

        protected override void FinishUseMeter()
        {
            if (Active)
            {
                Creature.Intimidation -= IntimidationIncrease;
                Active = false;
            }
        }

        public override void Update(GameTime time)
        {
            foreach (SubPart part in SubParts)
            {
                (part.Renderable as AnimateModel).Update(time);
            }
            base.Update(time);
        }
    }
}
