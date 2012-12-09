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
        public Bear(Vector3 position)
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
                new BearArms()                              // Part
                )
        {
            Scale = new Vector3(1.5f);
        }

        protected override List<PartBone> GetUsablePartBones()
        {
            List<PartBone> bones = new List<PartBone>();
            bones.Add(PartBone.ArmLeft1Cap);
            bones.Add(PartBone.ArmRight1Cap);
            return bones;
        }

        public override void Update(GameTime gameTime)
        {
            (mRenderable as AnimateModel).Update(gameTime);
            base.Update(gameTime);
        }
    }
}
