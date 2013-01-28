using System;
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
    public class Kangaroo : NonPlayerCreature
    {

        public Kangaroo(Vector3 position, Spawner spawn)
            : base(
                position,
                2.5f,                                           // Height
                1.0f,                                           // Radius
                10.0f,                                          // Mass
                CreatureConstants.KangarooSensitivityRadius,    // Sensitivity Radius
                new KangarooAI(),                               // AI
                new AnimateModel("kangaroo", "stand"),          // Model
                135,                                            // Vision Angle
                CreatureConstants.KangarooListeningSensitivity, // Listening Sensitivity
                CreatureConstants.KangarooSneak,                // Sneak
                CreatureConstants.KangarooIntimidation,         // Intimidation
                new KangarooLegs(),                              // Part
                spawn
                )
        {
            
        }

        protected override List<PartBone> GetUsablePartBones()
        {
            List<PartBone> bones = new List<PartBone>();
            bones.Add(PartBone.LegRearLeft1Cap);
            bones.Add(PartBone.LegRearRight1Cap);
            return bones;
        }

        protected virtual Matrix GetOptionalPartTransforms()
        {
            return Matrix.CreateScale(0.5f);
        }

    }
}
