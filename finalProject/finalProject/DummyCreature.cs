using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using BEPUphysics.Entities.Prefabs;
using GameConstructLibrary;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using finalProject.Parts;

namespace finalProject
{
    public class DummyCreature : NonPlayerCreature
    {
        public DummyCreature(Vector3 position)
            : base(position, 2.0f, 4.0f, 10.0f, 30.0f, new IntimidationAI(), new InanimateModel("box"), MathHelper.PiOver2, 10, 10, 1, 50, new CheetahLegs())
        {
            //(mRenderable as AnimateModel).PlayAnimation("Take 001");
            Scale = new Vector3(0.02f);
        }

        public override void Damage(int damage)
        {
            Move(Vector2.Zero);
            mIncapacitated = true;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            foreach (IGameObject i in CollidingObjects)
            {
                if (i is Creature)
                {
                    //Damage(1);
                }
            }
        }

        protected override List<Creature.PartBone> GetUsablePartBones()
        {
            List<Creature.PartBone> bones = new List<PartBone>();
            //bones.Add(PartBone.LegFrontLeft1Cap);

            return bones;
        }
    }
}
