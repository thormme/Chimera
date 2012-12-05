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
    public class Rhino : NonPlayerCreature
    {
        public Rhino(Vector3 position)
            : base(
                position,
                2.0f,                                           // Height
                3.0f,                                           // Radius
                30.0f,                                          // Mass
                CreatureConstants.RhinoSensitivityRadius,       // Sensitivity Radius
                new RhinoAI(),                             // AI
                new InanimateModel("box"),//new AnimateModel("rhino", "walk"),              // Model
                135,                                            // Vision Angle
                CreatureConstants.RhinoListeningSensitivity,    // Listening Sensitivity
                CreatureConstants.RhinoSneak,                   // Sneak
                CreatureConstants.RhinoIntimidation,            // Intimidation
                CreatureConstants.RhinoStartingHealth,			// Starting Health
                new RhinoHead()                                 // Part
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
            //(mRenderable as AnimateModel).Update(gameTime);
            base.Update(gameTime);
        }
    }
}
