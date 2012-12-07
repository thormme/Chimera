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
        public Cobra(Vector3 position)
            : base(
                position,
                3.0f,                                           // Height
                2.0f,                                           // Radius
                10.0f,                                          // Mass
                CreatureConstants.CobraSensitivityRadius,       // Sensitivity Radius
                new RangedAI(),                                  // AI
                new AnimateModel("cobra", "stand"),             // Model
                135,                                            // Vision Angle
                CreatureConstants.CobraListeningSensitivity,    // Listening Sensitivity
                CreatureConstants.CobraSneak,                   // Sneak
                CreatureConstants.CobraIntimidation,            // Intimidation
                new MindControlCobraHead()                      // Part
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
