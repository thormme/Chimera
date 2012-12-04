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
    public class Turtle : NonPlayerCreature
    {
        public Turtle(Vector3 position)
            : base(
                position,
                2.0f,                                           // Height
                2.0f,                                           // Radius
                6.0f,                                           // Mass
                CreatureConstants.TurtleSensitivityRadius,      // Sensitivity Radius
                new ActivationAI(),                             // AI
                new InanimateModel("box"),                      // Model
                135,                                            // Vision Angle
                CreatureConstants.TurtleListeningSensitivity,   // Listening Sensitivity
                CreatureConstants.TurtleSneak,                  // Sneak
                CreatureConstants.TurtleIntimidation,           // Intimidation
                CreatureConstants.BearStartingHealth,			// Starting Health
                new TurtleShell()                               // Part
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
