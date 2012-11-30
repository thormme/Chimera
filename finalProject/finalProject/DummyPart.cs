using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities.Prefabs;
using GraphicsLibrary;

namespace finalProject
{
    class DummyPart : CooldownPart
    {
        public DummyPart()
            : base(
            2.0f,
            new InanimateModel("kangaroo_leftLeg"),
            new Creature.PartBone[] { 
                Creature.PartBone.LegFrontLeft2Cap
                //Creature.PartBone.ArmLeft1Cap, 
                //Creature.PartBone.ArmLeft2Cap,
                //Creature.PartBone.Spine1Cap
            }, 
            1,
            new Vector3(),
            Matrix.CreateFromQuaternion(new Quaternion()),
            new Vector3(1.0f, 1.0f, 1.0f)
            )
        { }

        public override void UseCooldown(Vector3 direction)
        {
            Creature.Jump();
        }
    }
}
