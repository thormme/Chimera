using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.CollisionShapes.ConvexShapes;
using GameConstructLibrary;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Collidables;
using BEPUphysics.MathExtensions;

namespace Chimera
{
    /// <summary>
    /// A radial sensor used to identify PhysicsObjects near a point, like on a creature.
    /// </summary>
    public class ConeSensor : Sensor
    {
        private double mVisionAngle;

        protected List<Creature> mCollidingCreatures;
        public List<Creature> CollidingCreatures
        {
            get
            {
                return mCollidingCreatures;
            }
        }

        public virtual float Radius
        {
            get
            {
                return (Entity as Sphere).Radius;
            }
        }

        /// <summary>
        /// Constructs the RadialSensor as a sphere.
        /// </summary>
        /// <param name="radius">
        /// The radius of the RadialSensor's sphere.
        /// </param>
        public ConeSensor(MobileMesh body)
            : base(body)
        {
            mCollidingCreatures = new List<Creature>();
        }

        /// <summary>
        /// Stores the list of PhysicsObjects, and also a list of Creatures.
        /// </summary>
        public virtual void Update(GameTime time)
        {
            mCollidingCreatures.Clear();

            foreach (IGameObject gameObject in CollidingObjects)
            {
                Creature creature = gameObject as Creature;
                if (creature != null && !creature.Position.Equals(Position))
                {
                    mCollidingCreatures.Add(creature);
                }
            }

            mCollidingCreatures.Sort(new ClosestPhysicsObject(Position));
        }
    }
}
