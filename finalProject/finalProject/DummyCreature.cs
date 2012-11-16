using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using BEPUphysics.Entities.Prefabs;

namespace finalProject
{
    public class DummyCreature : NonPlayerCreature
    {
        private bool mIncapacitated;
        public override bool Incapacitated
        {
            get
            {
                return mIncapacitated;
            }
        }

        public DummyCreature(Vector3 position)
            : base(position, 20.0f, new AIController(), new InanimateModel("cube"), new Box(new Vector3(0), 10.0f, 10.0f, 10.0f, 1.0f), MathHelper.PiOver4, 10.0f, 10.0f, new DummyPart(position + new Vector3(25.0f, 0.0f, 0.0f)))
        { Scale = new Vector3(10.0f); }

        public override void Damage(int damage)
        {
            mIncapacitated = true;
        }

        public void Update(GameTime time)
        {
            base.Update(time);
        }
    }
}
