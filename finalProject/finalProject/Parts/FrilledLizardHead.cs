﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;

namespace finalProject.Parts
{
    class FrilledLizardHead : MeteredPart
    {
        private const int IntimidationIncrease = 5;
        private bool mActive = false;

        public FrilledLizardHead()
            : base(
                3.0f,
                5.0f,
                new Part.SubPart[] {
                    new SubPart(
                        new InanimateModel("sphere"),
                        new Creature.PartBone[] { 
                            Creature.PartBone.HeadCenterCap, 
                            Creature.PartBone.HeadLeftCap,
                            Creature.PartBone.HeadRightCap
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
            if (!mActive)
            {
                mActive = true;
                Creature.Intimidation += IntimidationIncrease;
            }
        }

        protected override void FinishUseMeter()
        {
            if (mActive)
            {
                Creature.Intimidation -= IntimidationIncrease;
                mActive = false;
            }
        }
    }
}
