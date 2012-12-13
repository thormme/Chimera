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
    public class Cobra : NonPlayerCreature
    {
        public Cobra(Vector3 position, Spawner spawn)
            : base(
                position,
                2.0f,                                           // Height
                2.0f,                                           // Radius
                10.0f,                                          // Mass
                CreatureConstants.CobraSensitivityRadius,       // Sensitivity Radius
                new RangedAI(),                                  // AI
                new AnimateModel("cobra", "stand"),             // Model
                135,                                            // Vision Angle
                CreatureConstants.CobraListeningSensitivity,    // Listening Sensitivity
                CreatureConstants.CobraSneak,                   // Sneak
                CreatureConstants.CobraIntimidation,            // Intimidation
                new CobraHead(),                      // Part
                spawn
                )
        {
            Scale = new Vector3(4.0f);

            new GameTip(
                new string[] 
                {
                    "You have encountered a cobra.",
                    "Cobras are able to charm their foes into a submissive state."
                },
                10.0f);

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
