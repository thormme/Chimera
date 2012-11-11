using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.CollisionShapes.ConvexShapes;

namespace GameConstructLibrary
{
    public class RadialSensor : PhysicsObject
    {
        protected List<Creature> mCollidingCreatures;
        public List<Creature>  CollidingCreatures { get { return mCollidingCreatures; } }

        protected List<PhysicsObject> mCollidingProps;
        public List<PhysicsObject> CollidingProps { get { return mCollidingProps; } }

        public RadialSensor(float radius)
            : base(null, new SphereShape(radius))
        {}

        public virtual void Collide(List<PhysicsObject> objects)
        {
            mCollidingCreatures.Clear();
            mCollidingProps.Clear();

            mCollidingProps = new List<PhysicsObject>(objects);
            foreach (PhysicsObject cur in objects)
            {
                if (cur as Creature != null)
                {
                    mCollidingCreatures.Add(cur as Creature);
                }
            }

            mCollidingCreatures.Sort(new ClosestPhysicsObject(Position));
        }
    }
}
