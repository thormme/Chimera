using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities.Prefabs;
using GraphicsLibrary;

namespace finalProject
{
    class DummyPart : Part
    {
        public DummyPart()
            : base(
            new InanimateModel("sphere"),
            new Creature.PartBone[] { 
                Creature.PartBone.ArmLeft1, 
                Creature.PartBone.LegFrontLeft1, 
                Creature.PartBone.L_Index1 
            }, 
            1,
            new Vector3(),
            Matrix.CreateFromQuaternion(new Quaternion()),
            new Vector3(1.0f)
            )
        {
        }
        public override void Update(GameTime time) { }

        public override void Use(Vector3 direction)
        {
            Creature.Jump();
        }
    }
}
