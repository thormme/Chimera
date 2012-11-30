﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using GameConstructLibrary;
using finalProject.Parts;

namespace finalProject.Creatures
{
    public class Kangaroo : NonPlayerCreature
    {
        public Kangaroo(Vector3 position)
            : base(
                position,
                3.0f,                            // Height
                2.0f,                            // Radius
                10.0f,                           // Mass
                10.0f,                           // Sensitivity Radius
                new IntimidationAI(),            // AI
                new InanimateModel("kangaroo"),  // Model
                MathHelper.PiOver4,              // Vision Angle
                10,                              // Listening Sensitivity
                3,                               // Sneak
                5,                               // Intimidation
                50,								 // Starting Health
                new KangarooLegs()               // Part
                )
        {
        }

        protected override List<PartBone> GetUsablePartBones()
        {
            List<PartBone> bones = new List<PartBone>();
            bones.Add(PartBone.LegFrontLeft1Cap);
            bones.Add(PartBone.LegRearLeft1Cap);
            bones.Add(PartBone.LegFrontRight1Cap);
            bones.Add(PartBone.LegRearRight1Cap);
            return bones;
        }
    }
}