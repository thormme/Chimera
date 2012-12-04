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
        public Kangaroo(Vector3 position)
            : base(
                position,
                3.0f,                                           // Height
                2.0f,                                           // Radius
                10.0f,                                          // Mass
                CreatureConstants.KangarooSensitivityRadius,    // Sensitivity Radius
                new KangarooAI(),                               // AI
                new InanimateModel("box"),//new AnimateModel("kangaroo", "walk"),           // Model
                135,                                            // Vision Angle
                CreatureConstants.KangarooListeningSensitivity, // Listening Sensitivity
                CreatureConstants.KangarooSneak,                // Sneak
                CreatureConstants.KangarooIntimidation,         // Intimidation
                CreatureConstants.KangarooStartingHealth,		// Starting Health
                new KangarooLegs()                              // Part
                )
        {
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
