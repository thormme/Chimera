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
                3.0f,                                   // Height
                2.0f,                                   // Radius
                10.0f,                                  // Mass
                15.0f,                                  // Sensitivity Radius
                new AggressiveAI(),                       // AI
                new InanimateModel("box"),   // Model
                135,                     // Vision Angle
                10,                                     // Listening Sensitivity
                3,                                      // Sneak
                7,                                      // Intimidation
                100,								        // Starting Health
                new BearArms()                      // Part
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
