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
    public class Eagle : NonPlayerCreature
    {
        public Eagle(Vector3 position)
            : base(
                position,
                3.0f,                                   // Height
                2.0f,                                   // Radius
                4.0f,                                   // Mass
                20.0f,                                  // Sensitivity Radius
                new EagleAI(),                     // AI
                new InanimateModel("box"),//new AnimateModel("eagle", "stand"),              // Model
                135,                                    // Vision Angle
				CreatureConstants.EagleListeningSensitivity,    // Listening Sensitivity
                CreatureConstants.EagleSneak,                   // Sneak
                CreatureConstants.EagleIntimidation,            // Intimidation
                CreatureConstants.EagleStartingHealth,		    // Starting Health
                new EagleWings()                          // Part
                )
        {
            Scale = new Vector3(4.0f);
        }

        protected override List<PartBone> GetUsablePartBones()
        {
            List<PartBone> bones = new List<PartBone>();
            //bones.Add(PartBone.LegRearLeft1Cap);
            //bones.Add(PartBone.LegRearRight1Cap);
            return bones;
        }

        public override void Update(GameTime gameTime)
        {
            //(mRenderable as AnimateModel).Update(gameTime);
            base.Update(gameTime);
        }
    }
}