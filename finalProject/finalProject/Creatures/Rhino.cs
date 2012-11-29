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
            : base(
            position,
            2.0f,
            1.0f,
            30.0f,
            20.0f,
            new IntimidationAI(),
            new InanimateModel("Rhino"),
            MathHelper.PiOver4,
            10,
            10,
            7,
            50,
            new /*RhinoHead*/DummyPart()
            )
        {
            Bones.Add(PartBone.HeadCenterCap);
        }
    }
}
