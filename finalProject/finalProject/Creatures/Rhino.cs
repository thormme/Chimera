using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using GameConstructLibrary;

namespace finalProject.Creatures
{
    class Rhino : NonPlayerCreature
    {
        public Rhino(Vector3 position)
            : base(position, 2.0f, 1.0f, 30.0f, 20.0f, new HostileAI(), new InanimateModel("Rhino"), MathHelper.PiOver4, 10.0f, 10.0f, new /*RhinoHead*/DummyPart())
        {

        }

        public override void Damage(int damage)
        {
            base.Damage(damage);
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
        }

        protected override List<Creature.PartBone> GetUsablePartBones()
        {
            List<Creature.PartBone> bones = new List<PartBone>();
            bones.Add(PartBone.HeadCenterCap);

            return bones;
        }
    }
}
