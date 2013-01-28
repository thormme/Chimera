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

        public Rhino(Vector3 position, Spawner spawn)
            : base(
                position,
                4.0f,                                           // Height
                1.5f,                                          // Radius
                30.0f,                                          // Mass
                CreatureConstants.RhinoSensitivityRadius,       // Sensitivity Radius
                new RhinoAI(),                                  // AI
                new AnimateModel("rhino", "stand"),              // Model
                135,                                            // Vision Angle
                CreatureConstants.RhinoListeningSensitivity,    // Listening Sensitivity
                CreatureConstants.RhinoSneak,                   // Sneak
                CreatureConstants.RhinoIntimidation,            // Intimidation
                new RhinoHead(),                                 // Part
                spawn
                )
        {
            Scale = new Vector3(2.0f);
        }

        protected override List<PartBone> GetUsablePartBones()
        {
            List<PartBone> bones = new List<PartBone>();
            bones.Add(PartBone.HeadCenterCap);
            return bones;
        }

        protected virtual Matrix GetOptionalPartTransforms()
        {
            return Matrix.CreateScale(0.5f);
        }

    }
}
