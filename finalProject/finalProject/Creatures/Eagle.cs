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

        public static GameTip mTip = new GameTip(
            new string[] 
            {
                "You have encountered an eagle.",
                "Eagles are able to soar through the sky."
            },
            10.0f); 

        public Eagle(Vector3 position, Spawner spawn)
            : base(
                position,
                3.0f,                                       // Height
                2.0f,                                       // Radius
                4.0f,                                       // Mass
                20.0f,                                      // Sensitivity Radius
                new EagleAI(),                              // AI
                new AnimateModel("eagle", "stand"),         // Model
                135,                                        // Vision Angle
				CreatureConstants.EagleListeningSensitivity,// Listening Sensitivity
                CreatureConstants.EagleSneak,               // Sneak
                CreatureConstants.EagleIntimidation,        // Intimidation
                new EagleWings(),                            // Part
                spawn
                )
        {
            Scale = new Vector3(4.0f);
        }

        protected override List<PartBone> GetUsablePartBones()
        {
            List<PartBone> bones = new List<PartBone>();
            bones.Add(PartBone.ArmLeft1Cap);
            bones.Add(PartBone.ArmRight1Cap);
            return bones;
        }

        protected override GameTip Tip
        {
            get
            {
                return mTip;
            }
        }

    }
}
