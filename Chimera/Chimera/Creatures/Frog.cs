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
    public class Frog : NonPlayerCreature
    {

        public Frog(Vector3 position, Spawner spawn)
            : base(
                position,
                2.0f,                                       // Height
                2.0f,                                       // Radius
                2.0f,                                       // Mass
                CreatureConstants.FrogSensitivityRadius,    // Sensitivity Radius
                new FrogAI(),                         // AI
                new AnimateModel("frog", "stand"),                  // Model
                135,                                        // Vision Angle
                CreatureConstants.FrogListeningSensitivity, // Listening Sensitivity
                CreatureConstants.FrogSneak,                // Sneak
                CreatureConstants.FrogIntimidation,         // Intimidation
                new FrogHead(),                              // Part
                spawn
                )
        {
            Scale = new Vector3(3.0f);
        }

        protected override List<PartBone> GetUsablePartBones()
        {
            List<PartBone> bones = new List<PartBone>();
            bones.Add(PartBone.HeadCenterCap);
            return bones;
        }

        protected virtual Matrix GetOptionalPartTransforms()
        {
            return Matrix.CreateScale(0.5f);
        }
        
    }
}