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
        public DummyPart(Vector3 position)
            : base(new InanimateModel("cube"), new Box(new Vector3(0.0f), 025.0f, 025.0f, 025.0f, 1.0f))
        {
            Position = position;
            Scale = new Vector3(025.0f, 025.0f, 025.0f);
        }
        public override void Update(GameTime time) { }

        public override void Use(Vector3 direction)
        {
            Creature.Jump();
        }
    }
}
