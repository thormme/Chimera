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
    public class Cheetah : NonPlayerCreature
    {
        public Cheetah(Vector3 position)
            : base(
                position,
                2.0f,                                           // Height
                2.0f,                                           // Radius
                6.0f,                                           // Mass
                CreatureConstants.CheetahSensitivityRadius,     // Sensitivity Radius
                new ActivationAI(),                             // AI
                new InanimateModel("box"),                      // Model
                135,                                            // Vision Angle
                CreatureConstants.CheetahListeningSensitivity,  // Listening Sensitivity
                CreatureConstants.CheetahSneak,                 // Sneak
                CreatureConstants.CheetahIntimidation,          // Intimidation
                CreatureConstants.CheetahStartingHealth,		// Starting Health
                new CheetahLegs()                               // Part
                )
        {
        }

        protected override List<PartBone> GetUsablePartBones()
        {
            List<PartBone> bones = new List<PartBone>();
            //bones.Add(PartBone.LegFrontLeft1Cap);
            //bones.Add(PartBone.LegRearLeft1Cap);
            //bones.Add(PartBone.LegFrontRight1Cap);
            //bones.Add(PartBone.LegRearRight1Cap);
            return bones;
        }
    }
}
