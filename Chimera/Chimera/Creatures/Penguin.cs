﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using GameConstructLibrary;
using Chimera.Parts;
using Chimera.AI;

namespace Chimera.Creatures
{
    public class Penguin : NonPlayerCreature
    {

        public Penguin(Vector3 position, Spawner spawn)
            : base(
                position,
                3.0f,                                           // Height
                2.0f,                                           // Radius
                6.0f,                                           // Mass
                CreatureConstants.PenguinSensitivityRadius,     // Sensitivity Radius
                new PenguinAI(),                                // AI
                new AnimateModel("penguin", "stand"),                      // Model
                135,                                            // Vision Angle
                CreatureConstants.PenguinListeningSensitivity,  // Listening Sensitivity
                CreatureConstants.PenguinSneak,                 // Sneak
                CreatureConstants.PenguinIntimidation,          // Intimidation
                new PenguinLimbs(),                              // Part
                spawn
                )
        {
            Scale = new Vector3(2.0f);
        }

        protected override List<PartBone> GetUsablePartBones()
        {
            List<PartBone> bones = new List<PartBone>();
            bones.Add(PartBone.LegRearLeft1Cap);
            bones.Add(PartBone.LegRearRight1Cap);
            bones.Add(PartBone.ArmLeft1Cap);
            bones.Add(PartBone.ArmRight1Cap);
            return bones;
        }
    }
}