﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using GameConstructLibrary;
using finalProject.Parts;
using finalProject.AI;

namespace finalProject.Creatures
{
    public class FrilledLizard : NonPlayerCreature
    {
        public FrilledLizard(Vector3 position)
            : base(
                position,
                2.0f,                       // Height
                2.0f,                       // Radius
                6.0f,                       // Mass
                15.0f,                      // Sensitivity Radius
                new FrilledLizardAI(),      // AI
                new AnimateModel("lizard", "stand"),  // Model
                MathHelper.PiOver4,         // Vision Angle
                10,                         // Listening Sensitivity
                8,                          // Sneak
                4,                          // Intimidation
                50,							// Starting Health
                new FrilledLizardHead()     // Part
                )
        {
            Scale = new Vector3(4.0f);
        }

        protected override List<PartBone> GetUsablePartBones()
        {
            List<PartBone> bones = new List<PartBone>();
            bones.Add(PartBone.HeadCenterCap);
            return bones;
        }

        public override void Update(GameTime gameTime)
        {
            (mRenderable as AnimateModel).Update(gameTime);
            base.Update(gameTime);
        }
    }
}
