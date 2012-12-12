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
    public class Bear : NonPlayerCreature
    {
        public Bear(Vector3 position, Spawner spawn)
            : base(
                position,
                1.8f,                                       // Height
                2.0f,                                       // Radius
                10.0f,                                      // Mass
                CreatureConstants.BearSensitivityRadius,    // Sensitivity Radius
                new AggressiveAI(),                         // AI
                new AnimateModel("bear", "stand"),                  // Model
                135,                                        // Vision Angle
                CreatureConstants.BearListeningSensitivity, // Listening Sensitivity
                CreatureConstants.BearSneak,                // Sneak
                CreatureConstants.BearIntimidation,         // Intimidation
                new BearArms(),                              // Part
                spawn
                )
        {
            Scale = new Vector3(2.0f);

            mTip = new GameTip(
                new string[] 
                {
                    "You have encountered a bear.",
                    "Be careful, these are beary strong enemies.",
                    "Even the strongest players bearly make it away with their lives.",
                    "Vanquish this foe and you will gain the ability to bear arms.",
                    "You will be able to maul other creatures with your bear hands."
                },
                10.0f);
        }

        protected override List<PartBone> GetUsablePartBones()
        {
            List<PartBone> bones = new List<PartBone>();
            bones.Add(PartBone.ArmLeft1Cap);
            bones.Add(PartBone.ArmRight1Cap);
            return bones;
        }

        protected virtual Matrix GetOptionalPartTransforms()
        {
            return Matrix.CreateScale(0.5f);
        }

    }
}
