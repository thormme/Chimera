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
    public class Penguin : NonPlayerCreature
    {
        public Penguin(Vector3 position)
            : base(
                position,
                3.0f,                                           // Height
                2.0f,                                           // Radius
                6.0f,                                           // Mass
                CreatureConstants.PenguinSensitivityRadius,     // Sensitivity Radius
                new PenguinAI(),                                // AI
                new InanimateModel("box"),                      // Model
                135,                                            // Vision Angle
                CreatureConstants.PenguinListeningSensitivity,  // Listening Sensitivity
                CreatureConstants.PenguinSneak,                 // Sneak
                CreatureConstants.PenguinIntimidation,          // Intimidation
                CreatureConstants.PenguinStartingHealth, 		// Starting Health
                new PenguinLimbs()                              // Part
                )
        {
            Scale = new Vector3(1.0f);
        }

        protected override List<PartBone> GetUsablePartBones()
        {
            List<PartBone> bones = new List<PartBone>();
            //bones.Add(PartBone.LegRearLeft1Cap);
            //bones.Add(PartBone.LegRearRight1Cap);
            return bones;
        }

        public override void Update(GameTime gameTime)
        {
            //(mRenderable as AnimateModel).Update(gameTime);
            base.Update(gameTime);
        }
    }
}
