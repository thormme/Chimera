using System;
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
    public class FrilledLizard : NonPlayerCreature
    {

        public static GameTip mTip = new GameTip(
            new string[] 
            {
                "You have encountered a frill necked lizard.",
                "Their venom will temporarily disables enemies."
            },
            10.0f);

        public FrilledLizard(Vector3 position, Spawner spawn)
            : base(
                position,
                2.0f,                                                   // Height
                2.0f,                                                   // Radius
                6.0f,                                                   // Mass
                CreatureConstants.FrilledLizardSensitivityRadius,       // Sensitivity Radius
                new RangedAI(),                                  // AI
                new AnimateModel("lizard", "stand"),  // Model
                135,                                                    // Vision Angle
                CreatureConstants.FrilledLizardListeningSensitivity,    // Listening Sensitivity
                CreatureConstants.FrilledLizardSneak,                   // Sneak
                CreatureConstants.FrilledLizardIntimidation,            // Intimidation
                new FrilledLizardHead(),                                 // Part
                spawn
                )
        {
            Scale = new Vector3(4.0f);
        }

        protected override List<PartBone> GetUsablePartBones()
        {
            List<PartBone> bones = new List<PartBone>();
            bones.Add(PartBone.HeadCenterCap);
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
