using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary
{
    public class RadialSensor : MyPhysicsObject
    {
        protected List<Creature> mCollidingCreatures;
        public List<Creature>  CollidingCreatures { get { return mCollidingCreatures; } }

        protected List<MyPhysicsObject> mCollidingProps;
        public List<MyPhysicsObject> CollidingProps { get { return mCollidingProps; } }

        public virtual void Collide(List<MyPhysicsObject> objects)
        {
            mCollidingCreatures.Clear();
            mCollidingProps.Clear();

            mCollidingProps = new List<MyPhysicsObject>(objects);
            foreach (MyPhysicsObject cur in objects)
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
