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
    /// <summary>
    /// A radial sensor used to identify PhysicsObjects near a point, like on a creature.
    /// </summary>
    public class RadialSensor : PhysicsObject
    {
        protected List<Creature> mCollidingCreatures;
        public List<Creature>  CollidingCreatures { get { return mCollidingCreatures; } }

        protected List<PhysicsObject> mCollidingProps;
        public List<PhysicsObject> CollidingProps { get { return mCollidingProps; } }

        /// <summary>
        /// Constructs the RadialSensor as a sphere.
        /// </summary>
        /// <param name="radius">
        /// The radius of the RadialSensor's sphere.
        /// </param>
        public RadialSensor(float radius)
            : base(null, new SphereShape(radius))
        {
            throw new NotImplementedException("I still need to set NoSolver.");
        }

        /// <summary>
        /// Stores the list of PhysicsObjects, and also a list of Creatures.
        /// </summary>
        /// <param name="objects">
        /// The list of colliding PhysicsObjects.
        /// </param>
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
