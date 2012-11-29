using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using GameConstructLibrary;

namespace finalProject.Creatures
{
    public class Rhino : NonPlayerCreature
    {
        public Rhino(Vector3 position)
            : base(position,
                   2.0f,                            // Height
                   3.0f,                            // Radius
                   30.0f,                           // Mass
                   20.0f,                           // Sensitivity Radius
                   new IntimidationAI(),            // AI
                   new InanimateModel("rhino"),     // Model
                   MathHelper.PiOver4,              // Vision Angle
                   10,                              // Listening Sensitivity
                   1,                               // Sneak
                   7,                               // Intimidation
                   50,								// Starting Health
                   new /*RhinoHead*/DummyPart())    // Part
            )
        {
            Bones.Add(PartBone.HeadCenterCap);
        }
    }
}
