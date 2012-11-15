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
            : base(new InanimateModel("cube"), new Box(new Vector3(0.0f), 0.25f, 0.25f, 0.25f))
        {
            Scale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        public override void Update(GameTime time) { }

        public override void Use(Vector3 direction) { }
    }
}
