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
    public class Frog : NonPlayerCreature
    {
        public Frog(Vector3 position)
            : base(
                position,
                3.0f,                                       // Height
                2.0f,                                       // Radius
                2.0f,                                       // Mass
                CreatureConstants.FrogSensitivityRadius,    // Sensitivity Radius
                new AggressiveAI(),                         // AI
                new AnimateModel("frog", "stand"),                  // Model
                135,                                        // Vision Angle
                CreatureConstants.FrogListeningSensitivity, // Listening Sensitivity
                CreatureConstants.FrogSneak,                // Sneak
                CreatureConstants.FrogIntimidation,         // Intimidation
                new FrogHead()                              // Part
                )
        {
            Scale = new Vector3(1.0f);
        }

        protected override List<PartBone> GetUsablePartBones()
        {
            List<PartBone> bones = new List<PartBone>();
            bones.Add(PartBone.HeadCenterCap);
            return bones;
        }

        public override void Update(GameTime gameTime)
        {
            (mRenderable as AnimateModel).Update(gameTime);
            base.Update(gameTime);
        }
    }
}
