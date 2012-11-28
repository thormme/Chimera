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
            : base(position, 2.0f, 0.25f, 10.0f, 10.0f, new HostileAI(), new InanimateModel("box"), MathHelper.PiOver4, 10.0f, 10.0f, new DummyPart(position + new Vector3(15.0f, 0.0f, 15.0f)))
        {
            Entity.Position = position;
            mIncapacitated = false;
        }

        public override void Damage(int damage)
        {
            mIncapacitated = true;
        }
    }
}
