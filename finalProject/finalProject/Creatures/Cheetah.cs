using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using GameConstructLibrary;
using finalProject.Parts;

namespace finalProject.Creatures
{
    public class Cheetah : NonPlayerCreature
    {
        public Cheetah(Vector3 position)
            : base(
                position,
                2.0f,                            // Height
                2.0f,                            // Radius
                6.0f,                            // Mass
                20.0f,                           // Sensitivity Radius
                new IntimidationAI(),            // AI
                new InanimateModel("cheetah"),   // Model
                MathHelper.PiOver4,              // Vision Angle
                10,                              // Listening Sensitivity
                8,                               // Sneak
                4,                               // Intimidation
                50,								 // Starting Health
                new CheetahLegs()                // Part
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
