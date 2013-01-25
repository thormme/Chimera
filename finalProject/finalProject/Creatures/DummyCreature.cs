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
using finalProject.AI;

namespace finalProject.Creatures
{
    public class DummyCreature : NonPlayerCreature
    {
        public DummyCreature(Vector3 position, Spawner spawn)
            : base(position, 2.0f, 4.0f, 10.0f, 30.0f, new PassiveAI(), new InanimateModel("box"), 135, 100, 10, 5, new EagleWings(), spawn)
        {
            //(mRenderable as AnimateModel).PlayAnimation("Take 001");
            Scale = new Vector3(1.0f);
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

        public override void AddTip()
        {
            
        }
    }
}
