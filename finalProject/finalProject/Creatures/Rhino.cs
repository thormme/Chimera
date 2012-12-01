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
    public class Rhino : NonPlayerCreature
    {
        public Rhino(Vector3 position)
            : base(
                position,
                2.0f,                            // Height
                3.0f,                            // Radius
                30.0f,                           // Mass
                20.0f,                           // Sensitivity Radius
                new IntimidationAI(),            // AI
                new AnimateModel("rhino", "walk"),     // Model
                MathHelper.PiOver4,              // Vision Angle
                10,                              // Listening Sensitivity
                1,                               // Sneak
                7,                               // Intimidation
                50,								 // Starting Health
                new RhinoHead()                  // Part
                )
        {
            Scale = new Vector3(0.02f);
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
